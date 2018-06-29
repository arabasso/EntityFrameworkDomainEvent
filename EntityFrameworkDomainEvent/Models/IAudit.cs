namespace EntityFrameworkDomainEvent.Models
{
    public interface IAudit
        : IEntity
    {
        string ToAudit();
    }
}