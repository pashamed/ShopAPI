using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();

        Task<CustomerDto?> GetCustomerByIdAsync(int id);

        Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto);

        Task<CustomerDto?> UpdateCustomerAsync(CustomerUpdateDto customerDto);

        Task<bool> DeleteCustomerAsync(int id);

        Task<IEnumerable<CustomerDto>> GetBirthdayCelebrantsAsync(DateOnly date);

        Task<IEnumerable<LastPurchaseDto>> GetRecentBuyersAsync(int days);

        Task<IEnumerable<CategoryPurchaseDto>> GetPopularCategoriesAsync(int customerId);
    }
}