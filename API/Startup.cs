using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


namespace API
{
     public class Startup
     {
          private readonly IConfiguration _config;
          public Startup(IConfiguration config)
          {
               _config = config;
          }

          // This method gets called by the runtime. Use this method to add services to the container.
          public void ConfigureServices(IServiceCollection services)
          {

               services.AddControllers();
               services.AddScoped<IProductRepository, ProductRepository>();
               services.AddAutoMapper(typeof(MappingProfiles));
               services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
               services.AddDbContext<StoreContext>(options =>
               {
                    options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
               });
               services.AddSwaggerGen(c =>
               {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
               });
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
          {
               app.UseMiddleware<ExceptionMiddleware>();
               app.UseSwagger();
               app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

               app.UseStatusCodePagesWithReExecute("/errors/{0}");
               app.UseHttpsRedirection();

               app.UseRouting();

               app.UseStaticFiles();

               app.UseAuthorization();

               app.UseEndpoints(endpoints =>
               {
                    endpoints.MapControllers();
               });
          }
     }
}
