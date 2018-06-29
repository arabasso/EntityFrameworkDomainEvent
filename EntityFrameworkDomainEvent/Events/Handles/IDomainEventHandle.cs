using System.Threading.Tasks;

namespace EntityFrameworkDomainEvent.Events.Handles
{
    public interface IDomainEventHandle<in T>
        where T : IDomainEvent
    {
        Task HandleAsync(T args);
    }
}
