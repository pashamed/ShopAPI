using FluentValidation;
using ShopApi.Core.Dtos;

namespace ShopAPI.Web.Validators
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.DateOfBirth)
                .NotEmpty();
        }
    }
}
