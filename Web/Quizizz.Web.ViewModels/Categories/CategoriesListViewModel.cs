namespace Quizizz.Web.ViewModels.Categories
{
    using System;

    using AutoMapper;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class CategoriesListViewModel : IMapFrom<Category>, IHaveCustomMappings
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizzesCount { get; set; }

        public string CreatedOnDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, CategoriesListViewModel>()
                .ForMember(
                x => x.QuizzesCount,
                opt => opt.MapFrom(x => x.Quizzes.Count));
        }
    }
}