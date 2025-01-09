using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();

        Task<CustomerDto?> GetCustomerByIdAsync(int id);

        Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);

        Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto customerDto);

        Task<bool> DeleteCustomerAsync(int id);

        Task<IEnumerable<CustomerDto>> GetBirthdayCelebrantsAsync(DateOnly date);

        Task<IEnumerable<CustomerDto>> GetRecentBuyersAsync(int days);

        Task<IEnumerable<string>> GetPopularCategoriesAsync(int customerId);
    }
}