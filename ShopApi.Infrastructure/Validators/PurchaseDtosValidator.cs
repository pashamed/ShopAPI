using FluentValidation;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;

namespace ShopApi.Infrastructure.Validators
{
    public class PurchaseCreateDtoValidator : AbstractValidator<PurchaseCreateDto>
    {
        public PurchaseCreateDtoValidator(
            ICustomerService customerService,
            IProductService productService,
            IPurchaseService purchaseService)
        {
            RuleFor(x => x.CustomerId).NotEmpty().MustAsync(async (id, _) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(id);
                return customer != null;
            }).WithMessage("Customer not found");
            RuleForEach(x => x.PurchaseItems).SetValidator(new PurchaseItemCreateDtoValidator(productService));
        }
    }

    public class PurchaseUpdateDtoValidator : AbstractValidator<PurchaseUpdateDto>
    {
        public PurchaseUpdateDtoValidator(
            ICustomerService customerService,
            IProductService productService,
            IPurchaseService purchaseService)
        {
            RuleFor(x => x.Id).NotEmpty().MustAsync(async (id, _) =>
            {
                var purchase = await purchaseService.GetPurchaseByIdAsync(id);
                return purchase != null;
            }).WithMessage("Purchase not found");
            RuleFor(x => x.CustomerId).NotEmpty().MustAsync(async (id, _) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(id);
                return customer != null;
            }).WithMessage("Customer not found");
            RuleFor(x => x.PurchaseItems).NotEmpty();
            RuleForEach(x => x.PurchaseItems).SetValidator(new PurchaseItemUpdateDtoValidator(productService));
        }
    }

    public class PurchaseItemCreateDtoValidator : AbstractValidator<PurchaseItemCreateDto>
    {
        public PurchaseItemCreateDtoValidator(IProductService productService)
        {
            RuleFor(x => x.ProductId).NotEmpty().MustAsync(async (id, _) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                return product != null;
            }).WithMessage("Product not found");
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    }

    public class PurchaseItemUpdateDtoValidator : AbstractValidator<PurchaseItemUpdateDto>
    {
        public PurchaseItemUpdateDtoValidator(IProductService productService)
        {
            RuleFor(x => x.ProductId).NotEmpty().MustAsync(async (id, _) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                return product != null;
            }).WithMessage("Product not found");
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    }
}