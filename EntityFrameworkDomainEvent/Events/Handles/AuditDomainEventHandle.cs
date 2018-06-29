using System.Threading.Tasks;
using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events.Handles
{
    public class AuditDomainEventHandle
        : IDomainEventHandle<AuditDomainEvent<IAudit>>
    {
        private readonly ApplicationDbContext _context;

        public AuditDomainEventHandle(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task HandleAsync(
            AuditDomainEvent<IAudit> args)
        {
            await _context.Set<Audit>().AddAsync(new Audit(args.Action, args.Audit));
            await _context.SaveChangesAsync();
        }
    }
}
