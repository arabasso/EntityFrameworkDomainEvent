using System.Collections.Generic;

namespace EntityFrameworkDomainEvent.Models
{
    public class Group
        : IEntity, IAudit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }

        protected Group()
        {
        }

        public Group(
            string name)
        {
            Name = name;
        }

        public string ToAudit()
        {
            return $"Name: {Name}";
        }
    }
}