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

        public async Task<IActionResult> AllCategoriesCreatedByTeacher(string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
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

        [HttpPost]
        public async Task<IActionResult> AssignQuizzesToCategory(CategoryWithQuizzesViewModel model)
        {
            var quizzesIds = model.Quizzes.Where(x => x.IsAssigned).Select(x => x.Id).ToList();

            if (quizzesIds.Count == 0)
            {
                model.Error = true;
                return this.View(model);
            }

            await this.categoriesService.AssignQuizzesToCategoryAsync(model.Id, quizzesIds);
            return this.RedirectToAction("CategoryDetails", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(string id)
        {
            var quizzes = await this.quizzesService.GetAllByCategoryIdAsync<QuizAssignViewModel>(id);
            var model = await this.categoriesService.GetByIdAsync<CategoryWithQuizzesViewModel>(id);
            model.Quizzes = quizzes;

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.categoriesService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.AllCategoriesCreatedByTeacher), "Categories");
        }

        public async Task<IActionResult> DeleteQuizFromCategory(string categoryId, string quizId)
        {
            await this.categoriesService.DeleteQuizFromCategoryAsync(categoryId, quizId);
            return this.RedirectToAction("CategoryDetails", "Categories", new { id = categoryId });
        }

        public IActionResult EditName(string id, string name)
        {
            var model = new EditCategoryNameInputViewModel
            {
                Id = id,
                Name = name,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditName(EditCategoryNameInputViewModel model)
        {
            await this.categoriesService.UpdateNameAsync(model.Id, model.Name);

            return this.RedirectToAction("CategoryDetails", "Categories", new { id = model.Id });
        }

    }
}
