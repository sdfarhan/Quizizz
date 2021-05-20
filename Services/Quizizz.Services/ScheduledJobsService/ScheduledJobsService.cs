using Quizizz.Data.Common.Repositories;
using Quizizz.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizizz.Services.ScheduledJobsService
{
    public class ScheduledJobsService
    {
        private readonly IDeletableEntityRepository<ScheduledJob> scheduledJobRepository;
        private readonly IDeletableEntityRepository<Event> eventServiceRepository;

        public ScheduledJobsService(
            IDeletableEntityRepository<ScheduledJob> scheduledJobRepository,
            IDeletableEntityRepository<Event> eventServiceRepository)
        {
            this.scheduledJobRepository = scheduledJobRepository;
            this.eventServiceRepository = eventServiceRepository;
        }

        public async Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay)
        {

        }
    }
}
