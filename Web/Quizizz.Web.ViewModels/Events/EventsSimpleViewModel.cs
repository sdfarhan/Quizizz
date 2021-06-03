namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Common.Enumerations;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class EventsSimpleViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Status Status { get; set; }
    }
}
