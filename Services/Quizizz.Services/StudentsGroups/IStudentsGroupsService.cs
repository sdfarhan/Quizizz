using System.Threading.Tasks;

namespace Quizizz.Services.StudentsGroups
{
    public interface IStudentsGroupsService
    {
        Task CreateStudentGroupAsync(string groupId, string studentId);
        Task DeleteAsync(string groupId, string studentId);
    }
}