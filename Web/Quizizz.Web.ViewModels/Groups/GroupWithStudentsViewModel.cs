namespace Quizizz.Web.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Students;

    public class GroupWithStudentsViewModel : IMapFrom<Group>
    {
        public GroupWithStudentsViewModel()
        {
            this.Students = new List<StudentsViewModel>();
        }

        public string Id { get; set; }

        public bool Error { get; set; }

        public IList<StudentsViewModel> Students { get; set; }
    }
}
