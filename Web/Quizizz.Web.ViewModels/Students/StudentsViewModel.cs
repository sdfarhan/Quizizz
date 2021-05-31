namespace Quizizz.Web.ViewModels.Students
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;

    public class StudentsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsAssigned { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, StudentsViewModel>()
                .ForMember(
                x => x.FullName,
                opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"));
        }
    }
}
