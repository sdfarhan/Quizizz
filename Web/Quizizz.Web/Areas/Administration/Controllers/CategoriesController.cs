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
    using Quizizz.Web.ViewModels.Categories;
    using Quizizz.Web.ViewModels.Quizzes;

    public class CategoriesController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;
        private readonly IQuizzesService quizzesService;

        public CategoriesController(
            UserManager<ApplicationUser> userManager,
            ICategoriesService categoriesService,
            IQuizzesService quizzesService)
        {
            this.userManager = userManager;
            this.categoriesService = categoriesService;
            this.quizzesService = quizzesService;
        }

        public async Task<IActionResult> AllCategoriesCreatedByTeacher(string searchCriteria, string searchText, int page = 0, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new CategoriesListAllViewModel
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allCategoriesCreatedByTeacherCount = await this.categoriesService.GetAllCategoriesCountAsync(userId, searchCriteria, searchText);
            if (allCategoriesCreatedByTeacherCount > 0)
            {
                var categories = await this.categoriesService.GetAllPerPage<CategoriesListViewModel>(page, countPerPage, userId, searchCriteria, searchText);
                model.Categories = categories;
                model.PagesCount = (int)Math.Ceiling(allCategoriesCreatedByTeacherCount / (decimal)countPerPage);
            }

            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var categoryId = await this.categoriesService.CreateCategoryAsync(model.Name, userId);

            return this.RedirectToAction("AssignQuizzesToCategory", new { id = categoryId });
        }

        public async Task<IActionResult> AssignQuizzesToCategory(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var quizzes = await this.quizzesService.GetUnAssignedToCategoryByCreatorIdAsync<QuizAssignViewModel>(id, userId);
            var model = await this.categoriesService.GetByIdAsync<CategoryWithQuizzesViewModel>(id);
            model.Quizzes = quizzes;

            return this.View(model);
        }
    }
}
