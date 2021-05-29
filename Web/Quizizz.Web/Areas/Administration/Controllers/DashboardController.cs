namespace Quizizz.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Data.Models;
    using Quizizz.Services.Data;
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels.Administration.Dashboard;
    using Quizizz.Web.ViewModels.UsersInRole;
    using System;
    using System.Threading.Tasks;

    public class DashboardController : AdministrationController
    {
        private const int PerPageDefaultValue = 5;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UsersService userService;

        public DashboardController(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            UsersService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> Index(
            string invalidEmail,
            string searchText,
            string searchCriteria,
            int page = 1,
            int countPerPage = PerPageDefaultValue)
        {
            var model = new DashboardIndexViewModel
            {
                NewUser = new UserInputViewModel(),
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            ApplicationRole role = null;
            if (searchCriteria == GlobalConstants.AdministratorRoleName || searchCriteria == GlobalConstants.TeacherRoleName)
            {
                role = await this.roleManager.FindByNameAsync(searchCriteria);
            }

            var userInRoleCount = await this.userService.GetAllInRolesCountAsync(searchCriteria, searchText, role?.Id);
            if (userInRoleCount > 0)
            {
                var users = await this.userService.GetAllInRolesPerPageAsync<UserInRoleViewModel>(
                    page, countPerPage, searchCriteria, searchText, role?.Id);
                foreach (var user in users)
                {
                    var appUser = await this.userManager.FindByIdAsync(user.Id);
                    var roles = await this.userManager.GetRolesAsync(appUser);
                    user.Role = string.Join(GlobalConstants.SplitOption, role);
                }

                model.Users = users;
                model.PagesCount = (int)Math.Ceiling(userInRoleCount / (decimal)countPerPage);
            }

            model.NewUser.IsNotAdded = invalidEmail != null ? true : false;
            model.NewUser.Email = invalidEmail ?? null;

            return this.View(model);
        }
    }
}
