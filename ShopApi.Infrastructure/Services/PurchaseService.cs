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

        public async Task<PurchaseDto> CreatePurchaseAsync(PurchaseDto purchaseDto)
        {
            var purchase = new Purchase
            {
                Date = purchaseDto.Date,
                TotalCost = purchaseDto.TotalCost,
                CustomerId = purchaseDto.CustomerId,
                PurchaseItems = purchaseDto.PurchaseItems.Select(pi => new PurchaseItem
                {
                    ProductId = pi.ProductId,
                    Quantity = pi.Quantity
                }).ToList()
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            purchaseDto.Id = purchase.Id;
            return purchaseDto;
        }

        public async Task<PurchaseDto?> UpdatePurchaseAsync(int id, PurchaseDto purchaseDto)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (purchase == null) return null;

            purchase.Date = purchaseDto.Date;
            purchase.TotalCost = purchaseDto.TotalCost;
            purchase.CustomerId = purchaseDto.CustomerId;

            // Update purchase items
            _context.PurchaseItems.RemoveRange(purchase.PurchaseItems);
            purchase.PurchaseItems = purchaseDto.PurchaseItems.Select(pi => new PurchaseItem
            {
                ProductId = pi.ProductId,
                Quantity = pi.Quantity
            }).ToList();

            await _context.SaveChangesAsync();
            return purchaseDto;
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
    }
}