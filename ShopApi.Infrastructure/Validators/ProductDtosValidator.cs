using FluentValidation;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;

namespace ShopApi.Infrastructure.Validators
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator(IProductService productService)
        {
            RuleFor(x => x.Id).MustAsync(async (id, _) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                return product != null;
            }).WithMessage("Product not found");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(20);
        }
    }

    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(20);
        }
    }
}