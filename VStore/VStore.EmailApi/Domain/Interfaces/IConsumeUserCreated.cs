using MassTransit;
using VStore.Shared.Contracts.Events;

namespace VStore.EmailApi.Domain.Interfaces
{
    public interface IConsumeUserCreated : IConsumer<UserCreatedEvent>
    {
    }
}
