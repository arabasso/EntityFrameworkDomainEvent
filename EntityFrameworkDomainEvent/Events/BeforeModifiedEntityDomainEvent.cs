using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class BeforeModifiedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public BeforeModifiedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}