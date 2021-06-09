namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Quizzes;

    public class EventWithQuizzesViewModel : IMapFrom<Event>
    {
        public EventWithQuizzesViewModel()
        {
            this.Quizzes = new List<QuizAssignViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Error { get; set; }

        public string TimeZone { get; set; }

        public IList<QuizAssignViewModel> Quizzes { get; set; }
    }
}
