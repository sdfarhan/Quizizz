// ReSharper disable VirtualMemberCallInConstructor
namespace Quizizz.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Quizizz.Data.Common.Models;

    public class Event : BaseDeletableModel<string>
    {
        public Event()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<EventGroup> EventGroups { get; set; }

        public virtual ICollection<ScheduledJob> ScheduledJobs { get; set; }
    }
}
