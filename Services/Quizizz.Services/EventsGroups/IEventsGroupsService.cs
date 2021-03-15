namespace Quizizz.Services.EventsGroups
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEventsGroupsService
    {
        Task CreateEventGroupAsync(string eventId, string groupId);

        Task DeleteAsync(string eventId, string groupId);
    }
}
