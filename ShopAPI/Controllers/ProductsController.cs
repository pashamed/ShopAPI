using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<ProductCreateDto> productCreateDtoValidator;
        private readonly IValidator<ProductUpdateDto> productUpdateDtoValidator;

        public ProductsController(
            IProductService productService,
            IValidator<ProductCreateDto> productCreateDtoValidator,
            IValidator<ProductUpdateDto> productUpdateDtoValidator)
        {
            _productService = productService;
            this.productCreateDtoValidator = productCreateDtoValidator;
            this.productUpdateDtoValidator = productUpdateDtoValidator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all products", Description = "Retrieves a list of all products.")]
        [SwaggerResponse(200, "A list of products", typeof(IEnumerable<ProductDto>))]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a product by ID", Description = "Retrieves a product by its ID.")]
        [SwaggerResponse(200, "The product with the specified ID", typeof(ProductDto))]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product", Description = "Creates a new product with the provided data.")]
        [SwaggerResponse(201, "The created product", typeof(ProductDto))]
        [SwaggerResponse(400, "Invalid product data")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            var res = await productCreateDtoValidator.ValidateAsync(productDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut()]
        [SwaggerOperation(Summary = "Updates an existing product", Description = "Updates an existing product with the provided data.")]
        [SwaggerResponse(200, "The updated product", typeof(ProductDto))]
        [SwaggerResponse(400, "Invalid product data")]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto productDto)
        {
            var res = await productUpdateDtoValidator.ValidateAsync(productDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var updatedProduct = await _productService.UpdateProductAsync(productDto);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a product by ID", Description = "Deletes a product by its ID.")]
        [SwaggerResponse(204, "Product deleted")]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}