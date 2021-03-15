namespace Quizizz.Services.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGroupsService
    {
        Task<IList<T>> GetAllAsync<T>(string creatorId = null, string eventId = null);

        Task<IList<T>> GetAllPerPageAsync<T>(int page, int countPerPage, string creatorId = null, string searchCriteria = null, string searchtext = null);

        Task<string> CreateGroupAsync(string name, string creatorId);
    }
}
