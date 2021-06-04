namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using Ganss.XSS;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Shared;

    public class QuizDetailsViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EventName { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string Password { get; set; }

        public int? Timer { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, QuizDetailsViewModel>()
                .ForMember(
                x => x.EventName,
                opt => opt.MapFrom(x => x.Event.Name))
                .ForMember(
                x => x.Password,
                opt => opt.MapFrom(x => x.Password.Content));
        }
    }
}
