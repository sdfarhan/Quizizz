namespace Quizizz.Services.Groups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGroupsService
    {
        Task AssignEventsToGroupAsync(string groupId, IList<string> evenstIds);

        Task AssignStudentsToGroupAsync(string groupId, IList<string> studentIds);

        Task<string> CreateGroupAsync(string name, string creatorId);

        Task DeleteAsync(string groupId);

        Task DeleteEventFromGroupAsync(string groupId, string eventId);

        Task DeleteStudentFromGroupAsync(string groupId, string studentId);

        Task<IList<T>> GetAllAsync<T>(string creatorId = null, string eventId = null);

        Task<IEnumerable<T>> GetAllByEventIdAsync<T>(string eventId);

        Task<int> GetAllGroupsCountAsync(string creatorId = null, string searchCriteria = null, string searchText = null);

        Task<IList<T>> GetAllPerPageAsync<T>(int page, int countPerPage, string creatorId = null, string searchCriteria = null, string searchText = null);

        Task<T> GetEventsFirstGroupAsync<T>(string eventId);

        Task<IList<T>> GetGroupModelAsync<T>(string groupId);

        Task UpdateNameAsync(string groupId, string name);
    }
}
