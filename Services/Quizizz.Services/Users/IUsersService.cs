using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quizizz.Services.Users
{
    public interface IUsersService
    {
        Task<bool> AddStudentAsync(string email, string teacherId);
        Task DeleteFromTeacherListAsync(string studentId, string teacherId);
        Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId);
        Task<int> GetAllInRolesCountAsync(string searchCriteria = null, string searchText = null, string roleId = null);
        Task<IList<T>> GetAllInRolesPerPageAsync<T>(int page, int countPerPage, string searchCriteria = null, string searchText = null, string roleId = null);
        Task<IList<T>> GetAllStudentsAsync<T>(string teacherId = null, string groupId = null);
        Task<int> GetAllStudentsCountAsync(string teacherId = null, string searchCriteria = null, string searchText = null);
        Task<IList<T>> GetAllStudentsCountAsync<T>(int page, int countPerPage, string teacherId = null, string searchCriteria = null, string searchText = null);
    }
}