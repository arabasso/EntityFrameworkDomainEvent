using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class BeforeSavedEntityDomainEvent<TEntity>
        : EntityDomainEvent<TEntity>
        where TEntity : IEntity
    {
        public BeforeSavedEntityDomainEvent(
            TEntity entity)
            : base(entity)
        {
        }
    }
}