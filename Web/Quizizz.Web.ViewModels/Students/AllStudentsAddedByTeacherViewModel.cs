namespace Quizizz.Web.ViewModels.Students
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllStudentsAddedByTeacherViewModel
    {
        public AllStudentsAddedByTeacherViewModel()
        {
            this.Students = new HashSet<StudentsViewModel>();
        }

        public string UserId { get; set; }

        public StudentInputViewModel NewStudent { get; set; }

        public IEnumerable<StudentsViewModel> Students { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public string SearchString { get; set; }

        public string SearchType { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage == this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage == 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }
    }
}
