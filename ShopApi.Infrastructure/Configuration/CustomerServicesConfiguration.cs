using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Services;
using ShopAPI.Infrastructure.Validators;

namespace ShopApi.Infrastructure.Configuration
{
    public static class CustomerServicesConfiguration
    {
        public static void AddCustomerServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IValidator<CustomerCreateDto>, CustomerCreateDtoValidator>();
            services.AddScoped<IValidator<CustomerUpdateDto>, CustomerUpdateDtoValidator>();
        }
    }
}