using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<ProductDto> CreateProductAsync(ProductDto productDto);

        Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto);

        Task<bool> DeleteProductAsync(int id);
    }
}