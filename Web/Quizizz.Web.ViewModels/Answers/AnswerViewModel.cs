namespace Quizizz.Web.ViewModels.Answers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Ganss.XSS;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Shared;

    public class AnswerViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(
            Modelvalidations.Answer.TextMaxLength,
            MinimumLength = Modelvalidations.Answer.TextMinLength,
            ErrorMessage = Modelvalidations.Error.RangeMessage)]
        public string Text { get; set; }

        public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);

        public bool IsRightAnswer { get; set; }

        public string QuestionId { get; set; }
    }
}
