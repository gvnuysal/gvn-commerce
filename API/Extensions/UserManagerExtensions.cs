﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByUserClaimsPrincipalWithAddressAsync(this UserManager<AppUser> input,ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var data= await input.Users.Include(x=>x.Address).SingleOrDefaultAsync(x=>x.Email==email);
            return data;
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            
            return await input.Users.Include(x=>x.Address).SingleOrDefaultAsync(x=>x.Email==email);
        }
    }
}