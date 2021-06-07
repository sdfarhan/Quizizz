namespace Quizizz.Web.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Events;
    using Quizizz.Web.ViewModels.Students;

    public class GroupDetailsViewModel : IMapFrom<Group>
    {
        public GroupDetailsViewModel()
        {
            this.Events = new HashSet<EventsAssignViewModel>();
            this.Students = new HashSet<StudentsViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<EventsAssignViewModel> Events { get; set; }

        public IEnumerable<StudentsViewModel> Students { get; set; }
    }
}
