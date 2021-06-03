namespace Quizizz.Web.ViewModels.Questions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Shared;

    public class QuestionInputModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(
            maximumLength: ModelValidations.Questions.TextMaxLength,
            MinimumLength = ModelValidations.Questions.TextMinLength,
            ErrorMessage = ModelValidations.Error.RangeMessage)]
        public string Text { get; set; }
    }
}
