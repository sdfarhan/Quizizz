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
    using Quizizz.Services.Tools.Expressions;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<Quiz> quizRepository;
        private readonly IExpressionBuilder expressionBuilder;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<Quiz> quizRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.categoryRepository = categoryRepository;
            this.quizRepository = quizRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<string> CreateCategoryAsync(string name, string creatorId)
        {
            var category = new Category()
            {
                Name = name,
                CreatorId = creatorId,
            };
            await this.categoryRepository.AddAsync(category);
            await this.categoryRepository.SaveChangesAsync();
            return category.Id;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        => await this.categoryRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task AssignQuizzesToCategoryAsync(string id, IEnumerable<string> quizzesIds)
        {
            var category = await this.categoryRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            foreach (var quizId in quizzesIds)
            {
                var quiz = await this.quizRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == quizId);
                category.Quizzes.Add(quiz);
                quiz.CategoryId = id;

                this.quizRepository.Update(quiz);
                this.categoryRepository.Update(category);
            }

            await this.categoryRepository.SaveChangesAsync();
            await this.quizRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var category = await this.GetByIdAsync<Category>(id);

            this.categoryRepository.Delete(category);
            await this.categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteQuizFromCategoryAsync(string categoryId, string quizId)
        {
            var category = await this.GetByIdAsync<Category>(categoryId);
            var quiz = await this.quizRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == quizId);

            category.Quizzes.Remove(quiz);
            quiz.CategoryId = null;

            this.categoryRepository.Update(category);
            this.quizRepository.Update(quiz);

            await this.categoryRepository.SaveChangesAsync();
            await this.quizRepository.SaveChangesAsync();
        }

        public async Task<int> GetAllCategoriesCountAsync(string creatorId, string searchCriteria = null, string searchText = null)
        {
            var query = this.categoryRepository.AllAsNoTracking()
                .Where(category => category.CreatorId == creatorId);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Category>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<T>> GetAllPerPage<T>(
            int page,
            int countPerPage,
            string creatorId,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = this.categoryRepository.AllAsNoTracking()
                .Where(category => category.CreatorId == creatorId);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Category>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByCreatorIdAsync<T>(string creatorId)
        => await this.categoryRepository
            .AllAsNoTracking()
            .Where(x => x.CreatorId == creatorId)
            .To<T>()
            .ToListAsync();

        public async Task UpdateNameAsync(string id, string newName)
        {
            var category = await this.GetByIdAsync<Category>(id);
            category.Name = newName;

            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
