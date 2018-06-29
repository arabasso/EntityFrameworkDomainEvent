using System;
using System.Threading.Tasks;
using EntityFrameworkDomainEvent.Models;

namespace EntityFrameworkDomainEvent.Events.Handles
{
    public class SendMailAfterAddedUserDomainEventHandle
        : IDomainEventHandle<AfterAddedEntityDomainEvent<User>>
    {
        public async Task HandleAsync(
            AfterAddedEntityDomainEvent<User> args)
        {
            Console.WriteLine("Sending mail to: " + args.Entity.Email + "...");

            await Task.CompletedTask;
        }
    }
}
