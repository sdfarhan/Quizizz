namespace Quizizz.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Quizizz.Data.Common.Enumerations;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Services.Tools.Expressions;

    public class EventsServices : IEventsService
    {
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IExpressionBuilder expressionBuilder;

        public EventsServices(
            IDeletableEntityRepository<Event> eventRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.eventRepository = eventRepository;
            this.expressionBuilder = expressionBuilder;
        }


        public Task AssignGroupsToEventAsync(string eventId, IList<string> groupIds)
        {
            throw new NotImplementedException();
        }

        public Task AssignQuizToEventAsync(string eventId, string quizId, string timeZone)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateEventAsync(string name, string activationDate, string activeFrom, string activateTo, string creatorId)
        {
            var activativationDateAndTime = this.GetActivationDateAndTimeUtc(activationDate, activeFrom);
            var durationActivity = this.GetDurationOfActivity(activationDate, activeFrom, activateTo);
            var newEvent = new Event
            {
                Name = name,
                Status = Status.Pending,
                ActivationDateAndTime = activativationDateAndTime,
                DurationOfActivity = durationActivity,
                CreatorId = creatorId,
            };

            await this.eventRepository.AddAsync(newEvent);
            await this.eventRepository.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task DeleteAsync(string eventId)
        {
            var eventTobeDeleted = await this.GetEventByIdAsync(eventId);
            var quizID = eventTobeDeleted.QuizId;

            if (quizID != null)
            {
                
            }

            this.eventRepository.Delete(eventTobeDeleted);
            await this.eventRepository.SaveChangesAsync();
        }

        public Task DeleteQuizFromEventAsync(string eventId, string quizId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<T>> GetAllByCreatorIdAsync<T>(string creatorId)
        => await this.eventRepository
                .AllAsNoTracking()
                .Where(x => x.CreatorId == creatorId)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

        public Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetAllEventsCountAsync(string creatorId = null, string searchCriteria = null, string searchText = null)
        {
            var query = this.eventRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Event>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public Task<IList<T>> GetAllFilteredByStatusAndGroup<T>(Status status, string groupId, string searchCriteria = null, string searchtext = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<T>> GetAllPerPage<T>(int page, int countPerPage, string creatorId = null, string searchCriteria = null, string searchText = null)
        {
            var query = this.eventRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Event>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetAllPerPageByCreatorIdAndStatus<T>(
            int page,
            int countPerPage,
            string creatorId,
            Status status,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = this.eventRepository.AllAsNoTracking().Where(x => x.Status == status && x.CreatorId == creatorId);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Event>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetEventsCountBycreatorIdAndStatusAsync(string creatorId, Status status, string searchCriteria = null, string searchText = null)
        {
            var query = this.eventRepository.AllAsNoTracking().Where(x => x.Status == status && x.CreatorId == creatorId);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Event>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public Task<int> GetEventsCountByStudentIdAndStatusAsync(string id, Status status, string searchCriteria = null, string searchtext = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetPerPageByStudentIdFilteredByStatusAsync<T>(Status status, string studentId, int page, int countPerPage, bool withDeleted, string searchCriteria = null, string searchtext = null)
        {
            throw new NotImplementedException();
        }

        public string GetTimeErrorMessage(string activeFrom, string activeTo, string activationDate, string timeZone)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToEventGroups(string eventId, string emailHtmlContent)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string id, string name, string activationDate, string activeFrom, string activeTo, string timeZone)
        {
            var eventTobeUpdated = await this.GetEventByIdAsync(id);
            var activationDateAndTime = this.GetActivationDateAndTimeUtc(activationDate, activeFrom);
            var durationOfActivity = this.GetDurationOfActivity(activationDate, activeFrom, activeTo);

            eventTobeUpdated.Name = name;
            eventTobeUpdated.ActivationDateAndTime = activationDateAndTime;
            eventTobeUpdated.DurationOfActivity = durationOfActivity;
            eventTobeUpdated.Status = this.GetStatus(activationDateAndTime, durationOfActivity, eventTobeUpdated.QuizId, timeZone);

            this.eventRepository.Update(eventTobeUpdated);
            await this.eventRepository.SaveChangesAsync();
            throw new NotImplementedException();
        }

        private TimeSpan GetDurationOfActivity(string activationDate, string activeFrom, string activateTo)
        {
            throw new NotImplementedException();
        }

        private DateTime GetActivationDateAndTimeUtc(string activationDate, string activeFrom)
        {
            throw new NotImplementedException();
        }

        private async Task<Event> GetEventByIdAsync(string id)
        => await this.eventRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        private Status GetStatus(DateTime activationDateAndTime, TimeSpan durationOfActivity, string quizId, string timeZone)
        {
            throw new NotImplementedException();
        }
    }
}
