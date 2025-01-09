using FluentValidation.Results;

namespace ShopApi.Web
{
    public static class ValidationErrorExtensions
    {

        public static IEnumerable<ValidationErrorResponse> ToValidationErrorResponse(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(e => new ValidationErrorResponse
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage
            });
        }
    }

    public class ValidationErrorResponse
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

}
