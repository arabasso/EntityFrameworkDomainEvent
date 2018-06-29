using EntityFrameworkDomainEvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDomainEvent
{
    public class EntityEntryCurrentState
    {
        public ActionEvent Action { get; }
        public IEntity Entity { get; }

        public EntityEntryCurrentState(
            ActionEvent action,
            IEntity entity)
        {
            Action = action;
            Entity = entity;
        }
    }
}