namespace Quizizz.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Common.Models;

    public class Group : BaseDeletableModel<string>
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<StudentGroup> StudentGroups { get; set; }

        public virtual ICollection<EventGroup> EventGroups { get; set; }
    }
}
