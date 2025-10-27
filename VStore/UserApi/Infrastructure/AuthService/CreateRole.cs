using BookReviewManager.Domain.ModelsAutentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Interfaces.IAuthService;

namespace BookReviewManager.Infrastructure.Service.Identity
{
    public class CreateRole : ICreateRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public CreateRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ResponseIdentityCreate> CreateRoleAsync(string roleName)
        {
            var roleExiste = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExiste)
            {
                var role = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (role.Succeeded)
                {
                    return new ResponseIdentityCreate { Message = $"Role - {roleName} created successfully!", Status = "200" };
                }
                else
                {
                    return new ResponseIdentityCreate { Status = "400", Message = "Error creating role.." };
                }
            }

            return new ResponseIdentityCreate { Status = "400", Message = "role already exists" };
        }
    }
}
