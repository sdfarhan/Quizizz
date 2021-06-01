namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Shared;

    public class QuizListViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int QuestionsCount { get; set; }

        public string CategoryName { get; set; }

        public string CreatedOnDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Color { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, QuizListViewModel>()
                .ForMember(
                x => x.QuestionsCount,
                opt => opt.MapFrom(x => x.Questions.Count))
                .ForMember(
                x => x.Color,
                opt => opt.MapFrom(x => x.EventId != null ? ModelConstants.ColorActive : ModelConstants.ColorEnded));
        }
    }
}
