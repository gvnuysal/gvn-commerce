using API.Helpers;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
     public static class AutoMapperServicesExtension
     {
          public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
          {
               services.AddAutoMapper(typeof(MappingProfiles));
               return services;
          }
     }
}