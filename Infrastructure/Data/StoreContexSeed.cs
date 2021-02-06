using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
     public class StoreContexSeed
     {
          public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
          {
               try
               {
                    if (!context.ProductBrands.Any())
                    {
                         var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                         var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                         foreach (var item in brands)
                         {
                              context.ProductBrands.Add(item);
                         }
                         await context.SaveChangesAsync();
                    }
                    if (!context.ProductTypes.Any())
                    {
                         var productTypesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                         var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                         foreach (var item in productTypes)
                         {
                              context.ProductTypes.Add(item);
                         }
                         await context.SaveChangesAsync();
                    }
                    if (!context.Products.Any())
                    {
                         var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                         var productTypes = JsonSerializer.Deserialize<List<Product>>(productData);
                         foreach (var item in productTypes)
                         {
                              context.Products.Add(item);
                         }
                         await context.SaveChangesAsync();
                    }
               }
               catch (System.Exception ex)
               {
                    var logger = loggerFactory.CreateLogger<StoreContexSeed>();
                    logger.LogError(ex.Message);
               }
          }
     }
}