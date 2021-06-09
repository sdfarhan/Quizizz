namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Groups;

    public class EventWithGroupsViewModel : IMapFrom<Event>
    {
        public EventWithGroupsViewModel()
        {
            this.Groups = new List<GroupAssignViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Error { get; set; }

        public IList<GroupAssignViewModel> Groups { get; set; }
    }
}
