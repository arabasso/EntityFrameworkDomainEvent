using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class BeforeDeletedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public BeforeDeletedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}