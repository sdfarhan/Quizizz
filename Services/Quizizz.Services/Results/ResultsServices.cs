namespace Quizizz.Services.Results
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

    public class ResultsServices : IResultsServices
    {
        private readonly IDeletableEntityRepository<Result> resultRepository;
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IExpressionBuilder expressionBuilder;

        public ResultsServices(
            IDeletableEntityRepository<Result> resultRepository,
            IDeletableEntityRepository<Event> eventRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.resultRepository = resultRepository;
            this.eventRepository = eventRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IEnumerable<T>> GetAllResultsByEventAndGroupPerPageAsync<T>(string eventId, string groupId, int page, int countPerPage)
        => await this.resultRepository
                     .AllAsNoTracking()
                     .Where(x => x.EventId == eventId)
                     .Where(x => x.Student.StudentsInGroups.Any(x => x.Group.Id == groupId))
                     .OrderBy(x => x.CreatedOn)
                     .Skip(countPerPage * (page - 1))
                     .Take(countPerPage)
                     .To<T>()
                     .ToListAsync();

        public async Task<int> GetAllResultsByEventAndGroupCountAsync(string eventId, string groupId)
        => await this.resultRepository
                 .AllAsNoTracking()
                 .Where(x => x.EventId == eventId)
                 .Where(x => x.Student.StudentsInGroups.Any(x => x.Group.Id == groupId))
                 .CountAsync();

        public async Task<string> CreateResultAsync(string studentId, int points, int maxPoints, string quizId)
        {
            var eventWithQuiz = await this.eventRepository
                .All()
                .Where(x => x.QuizId == quizId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    QuizName = x.Quiz.Name,
                    x.ActivationDateAndTime,
                })
                .FirstOrDefaultAsync();

            var result = new Result()
            {
                Points = points,
                StudentId = studentId,
                MaxPoints = maxPoints,
                EventId = eventWithQuiz.Id,
                EventName = eventWithQuiz.Name,
                QuizName = eventWithQuiz.QuizName,
                EventActivationDateAndTime = eventWithQuiz.ActivationDateAndTime,
            };

            await this.resultRepository.AddAsync(result);
            await this.resultRepository.SaveChangesAsync();

            var @event = await this.eventRepository
                .All()
                .Where(x => x.QuizId == quizId)
                .FirstOrDefaultAsync();
            @event.Results.Add(result);
            this.eventRepository.Update(@event);
            await this.eventRepository.SaveChangesAsync();

            return result.Id;
        }

        public async Task<IEnumerable<T>> GetPerPageByStudentIdAsync<T>(string id, int page, int countPerPage, string searchCriteria = null, string searchText = null)
        {
            var query = this.resultRepository.AllAsNoTracking().Where(x => x.StudentId == id);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Result>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByStudentIdAsync<T>(string id)
        => await this.resultRepository
            .AllAsNoTracking()
            .Where(x => x.StudentId == id)
            .OrderByDescending(x => x.CreatedOn)
            .To<T>()
            .ToListAsync();

        public async Task<int> GetResultsCountByStudentIdAsync(string id, string searchCriteria = null, string searchText = null)
        {
            var query = this.resultRepository.AllAsNoTracking().Where(x => x.StudentId == id);

            if (searchCriteria != null && searchText != null)
            {
                var filter = this.expressionBuilder.GetExpression<Result>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<string> GetQuizNameByEventIdAndStudentIdAsync(string eventId, string studentId)
        => await this.resultRepository.AllAsNoTracking()
                 .Where(x => x.EventId == eventId && x.StudentId == studentId)
                 .Select(x => x.QuizName)
                 .FirstOrDefaultAsync();
    }
}
