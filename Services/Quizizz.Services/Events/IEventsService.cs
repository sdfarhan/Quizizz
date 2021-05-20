namespace Quizizz.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Quizizz.Data.Common.Enumerations;

    public interface IEventsService
    {
        Task<IList<T>> GetAllByCreatorIdAsync<T>(string creatorId);

        Task<IList<T>> GetAllPerPage<T>(int page, int countPerPage, string creatorId = null, string searchCriteria = null, string searchText = null);

        Task<IList<T>> GetAllPerPageByCreatorIdAndStatus<T>(int page, int countPerPage, string creatorId, Status status, string searchCriteria = null, string searchtext = null);

        Task<int> GetAllEventsCountAsync(string creatorId = null, string searchCriteria = null, string searchtext = null);

        Task<int> GetEventsCountBycreatorIdAndStatusAsync(string creatorId, Status status, string searchCriteria = null, string searchtext = null);

        Task<IList<T>> GetAllFilteredByStatusAndGroup<T>(Status status, string groupId, string searchCriteria = null, string searchtext = null);

        Task<IList<T>> GetPerPageByStudentIdFilteredByStatusAsync<T>(
            Status status,
            string studentId,
            int page,
            int countPerPage,
            bool withDeleted,
            string searchCriteria = null,
            string searchtext = null);

        Task<int> GetEventsCountByStudentIdAndStatusAsync(string id, Status status, string searchCriteria = null, string searchtext = null);

        Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId);

        Task DeleteAsync(string id);

        Task UpdateAsync(string id, string name, string activationDate, string activeFrom, string activeTo, string timeZone);

        Task AssignQuizToEventAsync(string eventId, string quizId, string timeZone);

        Task AssignGroupsToEventAsync(string eventId, IList<string> groupIds);

        Task<string> CreateEventAsync(string name, string activationDate, string activeFrom, string activateTo, string creatorId);

        Task DeleteQuizFromEventAsync(string eventId, string quizId);

        string GetTimeErrorMessage(string activeFrom, string activeTo, string activationDate, string timeZone);

        Task SendEmailToEventGroups(string eventId, string emailHtmlContent);
    }
}
