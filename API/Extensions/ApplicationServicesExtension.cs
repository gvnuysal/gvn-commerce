using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration _config)
        {
            //services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            //services.AddScoped<IConnectionMultiplexer,ConnectionMultiplexer>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
               services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddDbContext<StoreContext>(options =>
            {
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                   {
                     var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                                                           .SelectMany(x => x.Value.Errors)
                                                           .Select(x => x.ErrorMessage).ToArray();
                     var errorResponse = new ApiValidationErrorResponse
                     {
                         Errors = errors
                     };

                     return new BadRequestObjectResult(errorResponse);
                 };

            });

            return services;
        }
    }
}