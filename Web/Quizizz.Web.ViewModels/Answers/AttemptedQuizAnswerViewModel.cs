namespace Quizizz.Web.ViewModels.Answers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Ganss.XSS;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class AttemptedQuizAnswerViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);

        public bool IsRightAnswerAssumption { get; set; }
    }
}
