using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Services;
using ShopApi.Infrastructure.Validators;

namespace ShopApi.Infrastructure.Configuration
{
    public static class PurchaseServiceConfiguration
    {
        public static void AddPurchaseServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IValidator<PurchaseCreateDto>, PurchaseCreateDtoValidator>();
            services.AddScoped<IValidator<PurchaseUpdateDto>, PurchaseUpdateDtoValidator>();
        }
    }
}