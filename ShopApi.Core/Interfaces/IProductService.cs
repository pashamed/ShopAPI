using ShopApi.Core.Dtos;

namespace ShopApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);

        Task<ProductDto?> UpdateProductAsync(ProductUpdateDto productDto);

        Task<bool> DeleteProductAsync(int id);
    }
}