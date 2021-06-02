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
    using Quizizz.Web.ViewModels.Groups;

    public class CategoriesController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;

        public CategoriesController(
            UserManager<ApplicationUser> userManager,
            ICategoriesService categoriesService)
        {
            this.userManager = userManager;
            this.categoriesService = categoriesService;
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

            }

            return View();
        }
    }
}
