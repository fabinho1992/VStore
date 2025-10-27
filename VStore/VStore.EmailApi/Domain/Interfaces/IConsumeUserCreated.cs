using MassTransit;
using UserApi.Domain.Events;

namespace VStore.EmailApi.Domain.Interfaces
{
    public interface IConsumeUserCreated : IConsumer<UserCreatedEvent>
    {
    }
}
