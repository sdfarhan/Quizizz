using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quizizz.Services.Questions
{
    public interface IQuestionsService
    {
        Task<string> CreateQuestionAsync(string quizId, string questionText);
        Task DeleteQuestionByIdAsync(string id);
        Task<IList<T>> GetAllByQuizIdAsync<T>(string id);
        Task<int> GetAllByQuizIdCountAsync(string id);
        Task<T> GetQuestionByQuizIdAndNumberAsync<T>(string quizId, int number);
        Task Update(string id, string text);
        Task UpdateAllQuestionsInQuizNumeration(string quizId);
    }
}