using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events
{
    public class AuditDomainEvent<TAudit>
        : IDomainEvent
        where TAudit : IEntity
    {
        public TAudit Audit { get; }
        public ActionEvent Action { get; }

        public AuditDomainEvent(
            TAudit audit,
            ActionEvent action)
        {
            Audit = audit;
            Action = action;
        }
    }
}