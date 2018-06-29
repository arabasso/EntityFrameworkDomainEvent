using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class AfterDeletedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public AfterDeletedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}