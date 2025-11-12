using BookReviewManager.Domain.ModelsAutentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.ModelsAutentication;

namespace UserApi.Domain.Interfaces.IAuthService
{
    public interface ICreateRole
    {
        Task<ResponseIdentityCreate> CreateRoleAsync(string roleName);
    }
}
