namespace Quizizz.Web.ViewModels.Questions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Ganss.XSS;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Answers;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public QuestionViewModel()
        {
            this.Answers = new List<AnswerViewModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }

        public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);

        public IList<AnswerViewModel> Answers { get; set; }

        public int Number { get; set; }
    }
}
