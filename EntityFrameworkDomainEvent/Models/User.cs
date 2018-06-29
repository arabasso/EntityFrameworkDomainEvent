namespace EntityFrameworkDomainEvent.Models
{
    public class User
        : IAudit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public Group Group { get; set; }

        protected User()
        {
        }

        public User(
            string name,
            string login,
            Group group)
        {
            Name = name;
            Login = login;
            Group = group;
        }

        public string ToAudit()
        {
            return $"Name: {Name}, Login: {Login}, E-mail: {Email}, Group: {Group.Name}";
        }
    }
}
