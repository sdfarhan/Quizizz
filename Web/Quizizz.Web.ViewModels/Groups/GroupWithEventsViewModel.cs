namespace Quizizz.Web.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Events;

    public class GroupWithEventsViewModel : IMapFrom<Group>
    {
        public GroupWithEventsViewModel()
        {
            this.Events = new List<EventsAssignViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Error { get; set; }

        public IList<EventsAssignViewModel> Events { get; set; }
    }
}
