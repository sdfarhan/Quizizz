namespace Quizizz.Services.EventsGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;

    public class EventsGroupsService : IEventsGroupsService
    {
        private readonly IDeletableEntityRepository<EventGroup> eventGroupReposiotry;

        public EventsGroupsService(IDeletableEntityRepository<EventGroup> eventGroupReposiotry)
        {
            this.eventGroupReposiotry = eventGroupReposiotry;
        }

        public async Task CreateEventGroupAsync(string eventId, string groupId)
        {
            var deletedEventGroup = await this.eventGroupReposiotry
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.EventId == eventId && x.GroupId == groupId)
                .FirstOrDefaultAsync();

            if (deletedEventGroup != null)
            {
                this.eventGroupReposiotry.Undelete(deletedEventGroup);
            }
            else
            {
                var eventGroup = new EventGroup
                {
                    EventId = eventId,
                    GroupId = groupId,
                };

                await this.eventGroupReposiotry.AddAsync(eventGroup);
            }

            await this.eventGroupReposiotry.SaveChangesAsync();
        }

        public async Task DeleteAsync(string eventId, string groupId)
        {
            var eventGroupToBeDeleted = await this.eventGroupReposiotry
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.EventId == eventId && x.GroupId == groupId)
                .FirstOrDefaultAsync();

            this.eventGroupReposiotry.Delete(eventGroupToBeDeleted);
            await this.eventGroupReposiotry.SaveChangesAsync();
        }
    }
}
