using System;
using System.IO;
using EntityFrameworkDomainEvent.Events;
using EntityFrameworkDomainEvent.Events.Handles;
using EntityFrameworkDomainEvent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkDomainEvent
{
    public static class Program
    {
        public static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DomainEventHandleService>();
            services.AddScoped<IDomainEventHandle<AfterAddedEntityDomainEvent<User>>, SendMailAfterAddedUserDomainEventHandle>();
            services.AddScoped<IDomainEventHandle<AuditDomainEvent<IAudit>>, AuditDomainEventHandle>();

            using(var serviceProvider = services.BuildServiceProvider())
            using (var context = serviceProvider.GetService<ApplicationDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Set<Group>().Add(new Group("Administrators"));

                var groupUsers = new Group("Users");

                context.Set<Group>().Add(groupUsers);

                var user = new User("Raphael Basso", "arabasso", groupUsers)
                {
                    Email = "arabasso@yahoo.com.br"
                };

                context.Set<User>().Add(user);

                context.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("Audit");
                Console.WriteLine("=====");
                Console.WriteLine();

                foreach (var audit in context.Set<Audit>())
                {
                    Console.WriteLine("Id: {0}", audit.Id);
                    Console.WriteLine("Action: {0}", audit.Action);
                    Console.WriteLine("Type: {0}", audit.Type);
                    Console.WriteLine("Date: {0}", audit.Date);
                    Console.WriteLine("Time: {0}", audit.Time);
                    Console.WriteLine("Description: {0}", audit.Description);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Press any key to continue. . .");
            Console.ReadKey();
        }
    }
}
