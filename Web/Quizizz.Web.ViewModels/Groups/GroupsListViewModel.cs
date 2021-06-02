namespace Quizizz.Web.ViewModels.Groups
{
    using System;

    using AutoMapper;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class GroupsListViewModel : IMapFrom<Group>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int StudentsCount { get; set; }

        public int EventsCount { get; set; }

        public string CreatedOnDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Group, GroupsListViewModel>()
                .ForMember(
                x => x.StudentsCount,
                opt => opt.MapFrom(x => x.StudentsGroups.Count))
                .ForMember(
                x => x.EventsCount,
                opt => opt.MapFrom(x => x.EventsGroups.Count));
        }
    }
}