using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quizizz.Services.Results
{
    public interface IResultsServices
    {
        Task<string> CreateResultAsync(string studentId, int points, int maxPoints, string quizId);
        Task<IEnumerable<T>> GetAllByStudentIdAsync<T>(string id);
        Task<int> GetAllResultsByEventAndGroupCountAsync(string eventId, string groupId);
        Task<IEnumerable<T>> GetAllResultsByEventAndGroupPerPageAsync<T>(string eventId, string groupId, int page, int countPerPage);
        Task<IEnumerable<T>> GetPerPageByStudentIdAsync<T>(string id, int page, int countPerPage, string searchCriteria = null, string searchText = null);
        Task<string> GetQuizNameByEventIdAndStudentIdAsync(string eventId, string studentId);
        Task<int> GetResultsCountByStudentIdAsync(string id, string searchCriteria = null, string searchText = null);
    }
}