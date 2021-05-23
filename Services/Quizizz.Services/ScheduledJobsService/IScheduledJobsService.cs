namespace Quizizz.Services.ScheduledJobsService
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IScheduledJobsService
    {
        Task DeleteJobsAsync(string id, bool v);
        Task CreateEndEventJobAsync(string eventId, TimeSpan endingDelay);
        Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay);
    }
}
