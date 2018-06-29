using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class EntityDomainEvent<TEntity>
        : IDomainEvent
        where TEntity : IEntity
    {
        public TEntity Entity { get; }

        public EntityDomainEvent(
            TEntity entity)
        {
            Entity = entity;
        }
    }
}