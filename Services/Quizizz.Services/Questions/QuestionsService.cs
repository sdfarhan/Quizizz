namespace Quizizz.Services.Questions
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

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Quiz> quizRepository;

        public QuestionsService(
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Quiz> quizRepository)
        {
            this.questionRepository = questionRepository;
            this.quizRepository = quizRepository;
        }

        public async Task<string> CreateQuestionAsync(string quizId, string questionText)
        {
            var quizzes = await this.quizRepository.AllAsNoTracking().ToListAsync();

            foreach (var quiz in quizzes)
            {
                foreach (var question in quiz.Questions)
                {
                    Console.WriteLine(question.ToString());
                }
            }

            return null;
            //var newQuestion = new Question
            //{
            //    Number = quiz.Questions.Count() + 1,
            //    QuizId = quizId,
            //    Text = questionText,
            //};

            //await this.questionRepository.AddAsync(newQuestion);
            //await this.questionRepository.SaveChangesAsync();

            //return newQuestion.Id;
        }

        public async Task DeleteQuestionByIdAsync(string id)
        {
            var question = await this.questionRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            this.questionRepository.Delete(question);
            await this.questionRepository.SaveChangesAsync();
            await this.UpdateAllQuestionsInQuizNumeration(question.QuizId);
        }

        public async Task UpdateAllQuestionsInQuizNumeration(string quizId)
        {
            var count = 0;

            var questions = this.questionRepository
              .AllAsNoTracking()
              .Where(x => x.QuizId == quizId)
              .OrderBy(x => x.Number);

            foreach (var question in questions)
            {
                question.Number = ++count;
                this.questionRepository.Update(question);
            }

            await this.questionRepository.SaveChangesAsync();
        }

        public async Task Update(string id, string text)
        {
            var questionToBeUpdated = this.questionRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == id);
            questionToBeUpdated.Text = text;
            this.questionRepository.Update(questionToBeUpdated);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task<IList<T>> GetAllByQuizIdAsync<T>(string id)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.QuizId == id)
            .OrderBy(x => x.Number)
            .To<T>()
            .ToListAsync();

        public async Task<int> GetAllByQuizIdCountAsync(string id)
        => await this.questionRepository.AllAsNoTracking().Where(x => x.QuizId == id).CountAsync();

        public async Task<T> GetQuestionByQuizIdAndNumberAsync<T>(string quizId, int number)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.QuizId == quizId && x.Number == number)
            .To<T>()
            .FirstOrDefaultAsync();
    }
}
