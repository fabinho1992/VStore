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
using UserApi.Domain.ModelsAutentication;
using VStore.Shared.Contracts.Events;

namespace UserApi.Infrastructure.Service.Identity
{
    public class CreateUser : ICreateUser
    {
        private readonly UserManager<User> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CreateUser> _logger;

        public CreateUser(UserManager<User> userManager, IPublishEndpoint publishEndpoint, ILogger<CreateUser> logger)
        {
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
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
            _logger.LogInformation("✅ Mensagem publicada com sucesso");

            return new ResponseIdentityCreate { Status = "Ok", Message = "User created successfully" };

            
        }
    }
}
