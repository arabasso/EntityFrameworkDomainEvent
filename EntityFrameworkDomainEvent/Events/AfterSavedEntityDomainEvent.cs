using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class AfterSavedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public AfterSavedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}