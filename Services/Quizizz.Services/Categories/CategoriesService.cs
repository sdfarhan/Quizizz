using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizizz.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        public Task AssignQuizzesToCategoryAsync(string id, IEnumerable<string> quizzesIds)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateCategoryAsync(string name, string creatorId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteQuizFromCategoryAsync(string categoryId, string quizId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAllCategoriesCountAsync(string creatorId, string searchCriteria = null, string searchText = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllPerPage<T>(int page, int countPerPage, string creatorId, string searchCriteria = null, string searchText = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetByCreatorIdAsync<T>(string creatorId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNameAsync(string id, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
