using System;
using System.Threading.Tasks;
using EntityFrameworkDomainEvent.Events;
using EntityFrameworkDomainEvent.Events.Handles;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkDomainEvent
{
    public class DomainEventHandleService
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventHandleService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RaiseAsync(
            Type domainEventType,
            object entity)
        {
            var @event = Activator.CreateInstance(domainEventType.MakeGenericType(entity.GetType()), entity);

            var type = typeof(IDomainEventHandle<>).MakeGenericType(@event.GetType());
            var method = type.GetMethod("HandleAsync");

            if (method == null) return;

            foreach (var handle in _serviceProvider.GetServices(type))
            {
                await (Task)method.Invoke(handle, new [] { @event });
            }
        }

        public void Raise(
            Type domainEventType,
            object entity)
        {
            RaiseAsync(domainEventType, entity).Wait();
        }

        public void Raise<T>(T @event)
            where T : IDomainEvent
        {
            RaiseAsync(@event).Wait();
        }

        public async Task RaiseAsync<T>(T @event)
            where T : IDomainEvent
        {
            foreach (var handle in _serviceProvider.GetServices<IDomainEventHandle<T>>())
            {
                await handle.HandleAsync(@event);
            }
        }
    }
}
