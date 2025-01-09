using FluentValidation;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;

namespace ShopAPI.Infrastructure.Validators
{
    public class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DateOfBirth).NotEmpty();
        }
    }

    public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator(ICustomerService customerService)
        {
            RuleFor(x => x.Id).MustAsync(async (id, _) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(id);
                return customer != null;
            }).WithMessage("Customer not found");
            RuleFor(x => x.FullName).MaximumLength(100);
            RuleFor(x => x.DateOfBirth).NotEmpty().When(x => x.DateOfBirth.HasValue);
        }
    }
}