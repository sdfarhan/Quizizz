namespace Quizizz.Services.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> repository;
        private readonly IDeletableEntityRepository<Quiz> quizRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> repository,
            IDeletableEntityRepository<Quiz> quizRepository)
        {
            this.repository = repository;
            this.quizRepository = quizRepository;
        }

        public async Task<string> CreateCategoryAsync(string name, string creatorId)
        {
            var category = new Category()
            {
                Name = name,
                CreatorId = creatorId,
            };
            await this.repository.AddAsync(category);
            await this.repository.SaveChangesAsync();
            return category.Id;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        => await this.repository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public Task AssignQuizzesToCategoryAsync(string id, IEnumerable<string> quizzesIds)
        {
            throw new NotImplementedException();
        }


        public async Task DeleteAsync(string id)
        {
            var category = await this.GetByIdAsync<Category>(id);

            this.repository.Delete(category);
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteQuizFromCategoryAsync(string categoryId, string quizId)
        {
            var category = await this.GetByIdAsync<Category>(categoryId);
        }

        public Task<int> GetAllCategoriesCountAsync(string creatorId, string searchCriteria = null, string searchText = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllPerPage<T>(int page, int countPerPage, string creatorId, string searchCriteria = null, string searchText = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetByCreatorIdAsync<T>(string creatorId)
        => await this.repository
            .AllAsNoTracking()
            .Where(x => x.CreatorId == creatorId)
            .To<T>()
            .ToListAsync();

        public async Task UpdateNameAsync(string id, string newName)
        {
            var category = await this.GetByIdAsync<Category>(id);
            category.Name = newName;

            this.repository.Update(category);
            await this.repository.SaveChangesAsync();
        }
    }
}
