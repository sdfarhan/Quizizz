namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Quizizz.Web.ViewModels.Categories;

    public class QuizzesAllListingViewModel
    {
        public QuizzesAllListingViewModel()
        {
            this.Categories = new List<CategorySimpleViewModel>();
            this.Quizzes = new List<QuizListViewModel>();
        }

        public IEnumerable<CategorySimpleViewModel> Categories { get; set; }

        public CategorySimpleViewModel CurrentCategory { get; set; }

        public IEnumerable<QuizListViewModel> Quizzes { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public string SearchType { get; set; }

        public string SearchString { get; set; }

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
