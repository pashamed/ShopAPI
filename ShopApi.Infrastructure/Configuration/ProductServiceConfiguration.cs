using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Services;
using ShopApi.Infrastructure.Validators;

namespace ShopApi.Infrastructure.Configuration
{
    public static class ProductServiceConfiguration
    {
        public static void AddProductServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
        }
    }
}