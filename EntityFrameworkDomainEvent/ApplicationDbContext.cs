using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkDomainEvent.Events;
using EntityFrameworkDomainEvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDomainEvent
{
    public class ApplicationDbContext
        : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Audit> Audit { get; set; }

        private readonly DomainEventHandleService _domainEventHandleService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            DomainEventHandleService domainEventHandleService)
            : base(options)
        {
            _domainEventHandleService = domainEventHandleService;
        }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(ba =>
            {
                ba.Property(p => p.Name).HasMaxLength(50).IsRequired();
                ba.Property(p => p.Login).HasMaxLength(35).IsRequired();
                ba.Property(p => p.Password).HasMaxLength(25);
                ba.Property(p => p.Email).HasMaxLength(100);
            });

            builder.Entity<Group>(ba =>
            {
                ba.Property(p => p.Name).HasMaxLength(50).IsRequired();
            });

            builder.Entity<Audit>(ba =>
            {
                ba.Property(p => p.Type).HasMaxLength(1024);
                ba.Property(p => p.Date);
                ba.Property(p => p.Time);
            });
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangedEntries();

            await RaiseBeforeEvents(entries);

            var result = await base.SaveChangesAsync(cancellationToken);

            await RaiseAfterEvents(entries);

            return result;
        }

        public override int SaveChanges()
        {
            var entries = ChangedEntries();

            RaiseBeforeEvents(entries).Wait();

            var result = base.SaveChanges();

            RaiseAfterEvents(entries).Wait();

            return result;
        }

        private async Task RaiseBeforeEvents(
            IEnumerable<EntityEntryCurrentState> entries)
        {
            foreach (var entry in entries)
            {
                switch (entry.Action)
                {
                    case ActionEvent.Deleted:
                        await _domainEventHandleService.RaiseAsync(typeof(BeforeDeletedEntityDomainEvent<>), entry.Entity);
                        break;
                    case ActionEvent.Modified:
                        await _domainEventHandleService.RaiseAsync(typeof(BeforeModifiedEntityDomainEvent<>), entry.Entity);
                        await _domainEventHandleService.RaiseAsync(typeof(BeforeSavedEntityDomainEvent<>), entry.Entity);
                        break;
                    case ActionEvent.Added:
                        await _domainEventHandleService.RaiseAsync(typeof(BeforeAddedEntityDomainEvent<>), entry.Entity);
                        await _domainEventHandleService.RaiseAsync(typeof(BeforeSavedEntityDomainEvent<>), entry.Entity);
                        break;
                }
            }
        }

        private async Task RaiseAfterEvents(
            IEnumerable<EntityEntryCurrentState> entries)
        {
            foreach (var entry in entries)
            {
                switch (entry.Action)
                {
                    case ActionEvent.Deleted:
                        await _domainEventHandleService.RaiseAsync(typeof(AfterDeletedEntityDomainEvent<>), entry.Entity);
                        break;
                    case ActionEvent.Modified:
                        await _domainEventHandleService.RaiseAsync(typeof(AfterModifiedEntityDomainEvent<>), entry.Entity);
                        await _domainEventHandleService.RaiseAsync(typeof(AfterSavedEntityDomainEvent<>), entry.Entity);
                        break;
                    case ActionEvent.Added:
                        await _domainEventHandleService.RaiseAsync(typeof(AfterAddedEntityDomainEvent<>), entry.Entity);
                        await _domainEventHandleService.RaiseAsync(typeof(AfterSavedEntityDomainEvent<>), entry.Entity);
                        break;
                }

                if (entry.Entity is IAudit audit)
                {
                    await _domainEventHandleService.RaiseAsync(new AuditDomainEvent<IAudit>(audit, entry.Action));
                }
            }
        }

        private List<EntityEntryCurrentState> ChangedEntries()
        {
            return ChangeTracker
                .Entries<IEntity>()
                .Where(w => States.Contains(w.State))
                .Select(s => new EntityEntryCurrentState(States2Action[s.State], s.Entity))
                .ToList();
        }

        private static readonly List<EntityState>
            States = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Deleted,
                EntityState.Modified
            };

        private static readonly Dictionary<EntityState, ActionEvent>
            States2Action = new Dictionary<EntityState, ActionEvent>
            {
                {EntityState.Added, ActionEvent.Added},
                {EntityState.Deleted, ActionEvent.Deleted},
                {EntityState.Modified, ActionEvent.Modified}
            };
    }
}
