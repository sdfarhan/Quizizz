namespace Quizizz.Services.Questions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionsService
    {
        Task<string> CreateQuestionAsync(string quizId, string questionText);

        Task DeleteQuestionByIdAsync(string id);

        Task<IList<T>> GetAllByQuizIdAsync<T>(string id);

        Task<int> GetAllByQuizIdCountAsync(string id);

        Task<T> GetByIdAsync<T>(string id);

        Task<T> GetQuestionByQuizIdAndNumberAsync<T>(string quizId, int number);

        Task Update(string id, string text);

        Task UpdateAllQuestionsInQuizNumeration(string quizId);
    }
}
