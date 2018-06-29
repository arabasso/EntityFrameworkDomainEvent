using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class AfterModifiedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public AfterModifiedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}
