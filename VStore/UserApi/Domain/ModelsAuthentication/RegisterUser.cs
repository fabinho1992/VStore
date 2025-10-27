using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewManager.Domain.ModelsAutentication
{
    public class RegisterUser
    {
        public RegisterUser(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
    }
}
