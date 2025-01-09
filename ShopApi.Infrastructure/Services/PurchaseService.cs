using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Dtos;
using ShopApi.Core.Entities;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ShopDbContext _context;

        public PurchaseService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Include(p => p.PurchaseItems)
                .Select(p => new PurchaseDto
                {
                    Id = p.Id,
                    Date = p.Date,
                    TotalCost = p.TotalCost,
                    CustomerId = p.CustomerId,
                    PurchaseItems = p.PurchaseItems.Select(pi => new PurchaseItemDto
                    {
                        Id = pi.Id,
                        ProductId = pi.ProductId,
                        Quantity = pi.Quantity
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PurchaseDto?> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (purchase == null) return null;

            return new PurchaseDto
            {
                Id = purchase.Id,
                Date = purchase.Date,
                TotalCost = purchase.TotalCost,
                CustomerId = purchase.CustomerId,
                PurchaseItems = purchase.PurchaseItems.Select(pi => new PurchaseItemDto
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    Quantity = pi.Quantity
                }).ToList()
            };
        }

        public async Task<PurchaseDto> CreatePurchaseAsync(PurchaseCreateDto purchaseDto)
        {
            var purchase = new Purchase
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TotalCost = purchaseDto.PurchaseItems?.Count > 0
                    ? purchaseDto.PurchaseItems.Sum(pi => pi.Quantity * _context.Products.Find(pi.ProductId)?.Price ?? 0)
                    : 0,
                CustomerId = purchaseDto.CustomerId,
                PurchaseItems = purchaseDto.PurchaseItems.Select(pi => new PurchaseItem
                {
                    ProductId = pi.ProductId,
                    Quantity = pi.Quantity
                }).ToList()
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return MapToPurchaseDto(purchase);
        }

        public async Task<PurchaseDto?> UpdatePurchaseAsync(PurchaseUpdateDto purchaseDto)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == purchaseDto.Id);
            if (purchase == null) return null;

            purchase.Date = purchaseDto.Date ?? purchase.Date;       
            purchase.CustomerId = purchaseDto.CustomerId;

            // Update purchase items and cost
            UpdatePurchaseItems(purchase, purchaseDto);
            purchase.TotalCost = purchaseDto.PurchaseItems?.Count > 0
                   ? purchaseDto.PurchaseItems.Sum(pi => pi.Quantity * _context.Products.Find(pi.ProductId)?.Price ?? 0)
                   : 0;

            await _context.SaveChangesAsync();
            return MapToPurchaseDto(purchase);
        }

        public async Task<bool> DeletePurchaseAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (purchase == null) return false;

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PurchaseDto MapToPurchaseDto(Purchase purchase)
        {
            return new PurchaseDto
            {
                Id = purchase.Id,
                Date = purchase.Date,
                TotalCost = purchase.TotalCost,
                CustomerId = purchase.CustomerId,
                PurchaseItems = purchase.PurchaseItems.Select(pi => new PurchaseItemDto
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    Quantity = pi.Quantity
                }).ToList()
            };
        }

        private void UpdatePurchaseItems(Purchase purchase, PurchaseUpdateDto purchaseUpdateDto)
        {
            var existingItemIds = purchaseUpdateDto.PurchaseItems.Select(pi => pi.Id).ToList();
            var itemsToRemove = purchase.PurchaseItems.Where(pi => !existingItemIds.Contains(pi.Id)).ToList();
            _context.PurchaseItems.RemoveRange(itemsToRemove);

            foreach (var itemDto in purchaseUpdateDto.PurchaseItems)
            {
                var existingItem = purchase.PurchaseItems.FirstOrDefault(pi => pi.Id == itemDto.Id);
                if (existingItem != null)
                {
                    existingItem.ProductId = itemDto.ProductId;
                    existingItem.Quantity = itemDto.Quantity;
                }
                else
                {
                    purchase.PurchaseItems.Add(new PurchaseItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity
                    });
                }
            }
        }
    }
}