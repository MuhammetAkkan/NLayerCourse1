using App.Services.ExceptionHandler;
using App.Services.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using App.Service.Filters;
using App.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<ICategoryService, CategoryService>();

        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        //fluent validation ekledik.
        services.AddFluentValidationAutoValidation();

        //
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly()); //AutoMapper ekledik.

        services.AddExceptionHandler<CriticalExceptionHandler>(); //exceptionHandler ekledik.
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddScoped(typeof(NotFoundFilter<,>)); //2 tane generic ifade aldığından virgül koymalıyız.

        return services;
        // Add your repository extensions here

    }
}


