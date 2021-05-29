namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Ganss.XSS;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class AttemptedQuizViewModel : IMapFrom<Quiz>
    {
        public AttemptedQuizViewModel()
        {
            this.Questions = new List<AttemptedQuizQuestionViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public int Timer { get; set; }

        public IList<AttemptedQuizQuestionViewModel> Questions { get; set; }
    }
}
