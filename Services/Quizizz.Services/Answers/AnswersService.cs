namespace Quizizz.Services.Answers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class AnswersService : IAnswersService
    {
        public Task CreateAnswerAsync(string answerText, bool isRightAnswer, string questionId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, string text, bool isRightAnswer)
        {
            throw new NotImplementedException();
        }
    }
}
