namespace Quizizz.Services.ScheduledJobsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Quizizz.Common;
    using Quizizz.Common.Hubs;
    using Quizizz.Data.Common.Enumerations;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;

    public class ScheduledJobsService : IScheduledJobsService
    {
        private readonly IDeletableEntityRepository<ScheduledJob> scheduledJobRepository;
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IHubContext<QuizHub> hub;
        private readonly IBackgroundJobClient backgroundJobClient;

        public ScheduledJobsService(
            IDeletableEntityRepository<ScheduledJob> scheduledJobRepository,
            IDeletableEntityRepository<Event> eventRepository,
            IHubContext<QuizHub> hub,
            IBackgroundJobClient backgroundJobClient)
        {
            this.scheduledJobRepository = scheduledJobRepository;
            this.eventRepository = eventRepository;
            this.hub = hub;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task CreateEndEventJobAsync(string eventId, TimeSpan endingDelay)
        {
            var activationScheduleJobId = this.backgroundJobClient.Schedule(() => this.SetStatusChangeJobAsync(eventId, Status.Ended), endingDelay);
            var job = new ScheduledJob
            {
                JobId = activationScheduleJobId,
                EventId = eventId,
                IsActivationJob = false,
            };

            await this.scheduledJobRepository.AddAsync(job);
            await this.scheduledJobRepository.SaveChangesAsync();
        }

        public async Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay)
        {
            var activationScheduleJobId = this.backgroundJobClient.Schedule(() => this.SetStatusChangeJobAsync(eventId, Status.Active), activationDelay);
            var job = new ScheduledJob
            {
                JobId = activationScheduleJobId,
                EventId = eventId,
                IsActivationJob = true,
            };

            await this.scheduledJobRepository.AddAsync(job);
            await this.scheduledJobRepository.SaveChangesAsync();
        }

        public async Task DeleteJobsAsync(string eventId, bool all, bool deleteActivationJobCondition = false)
        {
            var query = this.scheduledJobRepository
                .AllAsNoTracking()
                .Where(x => x.EventId == eventId);

            if (!all)
            {
                query = query.Where(x => x.IsActivationJob == deleteActivationJobCondition);
            }

            var jobsIds = await query
                .Select(x => x.JobId)
                .ToListAsync();

            foreach (var jobId in jobsIds)
            {
                this.backgroundJobClient.Delete(jobId);
            }
        }

        private async Task SetStatusChangeJobAsync(string eventId, Status status)
        {
            var @event = await this.eventRepository
                .AllAsNoTracking()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();

            var studentNames = await this.GetStudentsNamesByEventIdAsync(eventId);
            var adminUpdate = status == Status.Active ? "ActiveEventUpdate" : "EndedEventUpdate";
            var studentUpdate = status == Status.Active ? "NewActiveEventMessage" : "newEndedEventMessage";

            await this.hub.Clients.Group(GlobalConstants.AdministratorRoleName).SendAsync(adminUpdate, @event.Name);

            foreach (var name in studentNames.Distinct())
            {
                // hub code needs to go here
            }

            if (@event.QuizId == null || @event.Status == status)
            {
                return;
            }

            @event.Status = status;
            this.eventRepository.Update(@event);
            await this.eventRepository.SaveChangesAsync();

            // further hub code needs to go here
        }

        private async Task<string[]> GetStudentsNamesByEventIdAsync(string id)
        => await this.eventRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .SelectMany(x => x.EventsGroups.SelectMany(x => x.Group.StudentsGroups.Select(x => x.Student.UserName)))
            .ToArrayAsync();
    }
}
