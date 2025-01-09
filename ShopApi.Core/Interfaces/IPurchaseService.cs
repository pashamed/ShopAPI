using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync();

        Task<PurchaseDto?> GetPurchaseByIdAsync(int id);

        Task<PurchaseDto> CreatePurchaseAsync(PurchaseCreateDto purchaseDto);

        Task<PurchaseDto?> UpdatePurchaseAsync(PurchaseUpdateDto purchaseDto);

        Task<bool> DeletePurchaseAsync(int id);
    }
}