using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class BeforeAddedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public BeforeAddedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}