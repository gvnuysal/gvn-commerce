using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        { 
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Güven",
                    Email = "gvnuysal@gmail.com",
                    Address = new Address()
                    {
                        FirstName = "Güven",
                        LastName = "Uysal",
                        Street = "10 The Street",
                        City = "İstanbul",
                        ZipCode = "90084545"
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}