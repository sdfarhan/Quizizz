namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using AutoMapper;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Questions;
    using Quizizz.Web.ViewModels.Shared;

    public class InputQuizViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public InputQuizViewModel()
        {
            this.Questions = new List<QuestionInputViewModel>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(
            maximumLength: ModelValidations.Quizzes.NameMaxLength,
            MinimumLength = ModelValidations.Quizzes.NameMinLength,
            ErrorMessage = ModelValidations.Error.RangeMessage)]
        public string Name { get; set; }

        public string CreatorId { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(
            maximumLength: ModelValidations.Password.MaxLength,
            MinimumLength = ModelValidations.Password.MinLength,
            ErrorMessage = ModelValidations.Error.RangeMessage)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int? Timer { get; set; }

        public bool PasswordIsValid { get; set; }

        public IList<QuestionInputViewModel> Questions { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, InputQuizViewModel>()
                .ForMember(
                x => x.Password,
                opt => opt.MapFrom(x => x.Password.Content));
        }
    }
}
