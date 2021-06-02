namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Models;
    using Quizizz.Services.Categories;
    using Quizizz.Services.Quizzes;
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels.Categories;
    using Quizizz.Web.ViewModels.Quizzes;

    public class QuizzesController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;
        private readonly IQuizzesService quizzesService;

        public QuizzesController(
            UserManager<ApplicationUser> userManager,
            ICategoriesService categoriesService,
            IQuizzesService quizzesService)
        {
            this.userManager = userManager;
            this.categoriesService = categoriesService;
            this.quizzesService = quizzesService;
        }

        public async Task<IActionResult> AllQuizzesCreatedByTeacher(string categoryId, string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new QuizzesAllListingViewModel
            {
                Categories = await this.categoriesService.GetByCreatorIdAsync<CategorySimpleViewModel>(userId),
                CurrentCategory = categoryId == null ? null : await this.categoriesService.GetByIdAsync<CategorySimpleViewModel>(categoryId),
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allQuizzesCreatedByTeacherCount = await this.quizzesService.GetAllQuizzesCountAsync(userId, searchCriteria, searchText, categoryId);
            if (allQuizzesCreatedByTeacherCount > 0)
            {
                var quizzes = await this.quizzesService.GetAllPerPageAsync<QuizListViewModel>(page, countPerPage, userId, searchCriteria, searchText, categoryId);
                model.Quizzes = quizzes;
                model.PagesCount = (int)Math.Ceiling(allQuizzesCreatedByTeacherCount / (decimal)countPerPage);
            }

            return this.View(model);
        }
    }
}
