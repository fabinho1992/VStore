using MassTransit;
using VStore.EmailApi.Application.UserHttpClient;
using VStore.EmailApi.Domain.Interfaces;
using VStore.EmailApi.Infrastructure.Consumers;
using VStore.EmailApi.Infrastructure.EmailService;
using VStore.EmailApi.Infrastructure.ServiceEmail;
using VStore.Shared.Contracts.Events;

namespace VStore.EmailApi.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISendEmail, SendEmail>();
            services.AddScoped<IConsumeUserCreated, UserCreatedConsumer>();
            services.AddScoped<IHttpClientUser, HttpClientUser>();

            return services;
        }

        //public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddMassTransit(x =>
        //    {
        //        x.AddConsumer<UserCreatedConsumer>();
        //        x.AddConsumer<OrderCreatedConsumer>();

        //        x.UsingRabbitMq((context, cfg) =>
        //        {
        //            cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
        //            {
        //                h.Username(configuration["RabbitMQ:Username"]);
        //                h.Password(configuration["RabbitMQ:Password"]);
        //            });

        //            // TAMBÉM customizar nome da exchange no consumidor
        //            cfg.Message<UserCreatedEvent>(e =>
        //                e.SetEntityName("user-created-events"));

        //            cfg.Message<OrderCreatedEvent>(e =>
        //                e.SetEntityName("order-created-events"));

        //            cfg.ReceiveEndpoint("user-created-email", e =>
        //            {
        //                e.Bind("user-created-events"); // ← Binding manual
        //                e.ConfigureConsumer<UserCreatedConsumer>(context);
        //            });

        //            cfg.ReceiveEndpoint("order-created-email", e =>
        //            {
        //                e.Bind("order-created-events"); // ← Binding manual
        //                e.ConfigureConsumer<OrderCreatedConsumer>(context);
        //            });
        //        });
        //    });

        //    return services;
        //}
        public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<OrderCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    // CONFIGURAÇÃO PARA LIDAR COM NAMESPACES DIFERENTES
                    cfg.Message<OrderCreatedEvent>(e =>
                    {
                        e.SetEntityName("order-created-events"); // Mesmo nome do publisher
                    });

                    cfg.Message<UserCreatedEvent>(e =>
                    {
                        e.SetEntityName("user-created-events"); // Mesmo nome do publisher
                    });

                    // CONFIGURAÇÃO DOS ENDPOINTS
                    cfg.ReceiveEndpoint("user-created-email", e =>
                    {
                        e.ConfigureConsumer<UserCreatedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("order-created-email", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}
