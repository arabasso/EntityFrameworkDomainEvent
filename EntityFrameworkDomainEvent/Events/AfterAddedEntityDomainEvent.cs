using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class AfterAddedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public AfterAddedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}