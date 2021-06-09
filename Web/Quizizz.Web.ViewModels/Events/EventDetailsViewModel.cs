﻿namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using Quizizz.Data.Common.Enumerations;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Groups;

    public class EventDetailsViewModel : IMapFrom<Event>, IHaveCustomMappings
    {
        public EventDetailsViewModel()
        {
            this.Groups = new HashSet<GroupAssignViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Status Status { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public string ActivationDate { get; set; }

        public string ActiveFrom { get; set; }

        public string ActiveTo { get; set; }

        public string QuizName { get; set; }

        public string QuizId { get; set; }

        public string ConfirmationMessage { get; set; }

        public IEnumerable<GroupAssignViewModel> Groups { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, EventDetailsViewModel>()
            .ForMember(
                  x => x.QuizName,
                  opt => opt.MapFrom(x => x.Quiz.Name));
        }
    }
}
