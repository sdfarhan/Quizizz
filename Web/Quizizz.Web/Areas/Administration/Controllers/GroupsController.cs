namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Models;
    using Quizizz.Services.Groups;
    using Quizizz.Web.ViewModels.Groups;

    public class GroupsController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGroupsService groupsService;

        public GroupsController(
            UserManager<ApplicationUser> userManager,
            IGroupsService groupsService)
        {
            this.userManager = userManager;
            this.groupsService = groupsService;
        }

        public async Task<IActionResult> AllGroupsCreatedByTeacher(string searchCriteria, string searchText, int page = 0, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new GroupsListAllViewModel
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allGroupsCreatedByTeacherCount = await this.groupsService.GetAllGroupsCountAsync(userId, searchCriteria, searchText);
            if (allGroupsCreatedByTeacherCount > 0)
            {
                var groups = await this.groupsService.GetAllPerPageAsync<GroupsListViewModel>(page, countPerPage, userId, searchCriteria, searchText);
                model.Groups = groups;
                model.PagesCount = (int)Math.Ceiling(allGroupsCreatedByTeacherCount / (decimal)countPerPage);
            }

            return this.View(model);
        }
    }
}
