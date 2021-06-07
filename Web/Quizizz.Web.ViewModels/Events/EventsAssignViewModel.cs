namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class EventsAssignViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public bool IsAssigned { get; set; }
    }
}
