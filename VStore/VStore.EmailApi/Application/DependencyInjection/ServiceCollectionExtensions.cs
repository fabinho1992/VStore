using MassTransit;
using UserApi.Domain.Interfaces.IEmailServices;
using UserApi.Infrastructure.EmailService;
using VStore.EmailApi.Infrastructure.Consumers;

namespace VStore.EmailApi.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISendEmail, SendEmail>();

            return services;
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("user-created-email", e =>
                    {
                        e.ConfigureConsumer<UserCreatedConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}
