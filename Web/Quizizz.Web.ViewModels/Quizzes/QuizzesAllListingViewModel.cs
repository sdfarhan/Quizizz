﻿namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class QuizzesAllListingViewModel
    {
        public QuizzesAllListingViewModel()
        {
            this.Quizzes = new List<QuizListViewModel>();
        }

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
