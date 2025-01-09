using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync();

        Task<PurchaseDto?> GetPurchaseByIdAsync(int id);

        Task<PurchaseDto> CreatePurchaseAsync(PurchaseDto purchaseDto);

        Task<PurchaseDto?> UpdatePurchaseAsync(int id, PurchaseDto purchaseDto);

        Task<bool> DeletePurchaseAsync(int id);
    }
}