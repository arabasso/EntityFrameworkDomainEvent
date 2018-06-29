using System;

namespace EntityFrameworkDomainEvent.Models
{
    public class Audit
        : IEntity
    {
        public long Id { get; set; }
        public string Type { get; private set; }
        public ActionEvent Action { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan Time { get; private set; }

        protected Audit()
        {
        }

        public Audit(
            ActionEvent action,
            IAudit audit)
        {
            Action = action;
            Type = audit.GetType().ToString();
            Description = audit.ToAudit();

            var now = DateTime.Now;

            Date = now.Date;
            Time = now.TimeOfDay;
        }
    }
}