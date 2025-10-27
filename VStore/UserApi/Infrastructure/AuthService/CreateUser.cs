using BookReviewManager.Domain.ModelsAutentication;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Events;
using UserApi.Domain.Interfaces.IAuthService;

namespace BookReviewManager.Infrastructure.Service.Identity
{
    public class CreateUser : ICreateUser
    {
        private readonly UserManager<User> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateUser(UserManager<User> userManager, IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ResponseIdentityCreate> CreateUserAsync(RegisterUser registerUser)
        {
            var usuarioExiste = await _userManager.FindByEmailAsync(registerUser.Email!);//consulto se o nome passado exite no banco

            if (usuarioExiste != null)
            {
                return new ResponseIdentityCreate { Status = "Erro", Message = "User already exists!" };// se existir passo esse erro
            }
            User user = new()
            {
                DisplayName = registerUser.UserName,
                UserName = registerUser.Email,
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var resultado = await _userManager.CreateAsync(user, registerUser.Password!);//crio o usuario

            if (!resultado.Succeeded)
            {
                return new ResponseIdentityCreate { Message = "Erro", Status = "Error creating user" };
            }

            await _publishEndpoint.Publish(new UserCreatedEvent
            {
                UserId = Guid.Parse(user.Id),
                Email = user.Email,
                UserName = user.UserName,
                CreatedAt = DateTime.UtcNow
            });

            return new ResponseIdentityCreate { Status = "Ok", Message = "User created successfully" };

            
        }
    }
}
