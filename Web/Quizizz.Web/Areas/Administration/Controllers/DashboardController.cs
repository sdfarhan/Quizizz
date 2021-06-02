namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Data.Models;
    using Quizizz.Services.Data;
    using Quizizz.Services.Events;
    using Quizizz.Services.Groups;
    using Quizizz.Services.Quizzes;
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels.Administration.Dashboard;
    using Quizizz.Web.ViewModels.Events;
    using Quizizz.Web.ViewModels.Groups;
    using Quizizz.Web.ViewModels.Quizzes;
    using Quizizz.Web.ViewModels.Students;
    using Quizizz.Web.ViewModels.UsersInRole;

    public class DashboardController : AdministrationController
    {
        private const int PerPageDefaultValue = 5;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEventsService eventsService;
        private readonly IGroupsService groupsService;
        private readonly IQuizzesService quizzesService;
        private readonly IUsersService userService;

        public DashboardController(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IEventsService eventsService,
            IGroupsService groupsService,
            IQuizzesService quizzesService,
            IUsersService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.eventsService = eventsService;
            this.groupsService = groupsService;
            this.quizzesService = quizzesService;
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
                    user.Role = string.Join(GlobalConstants.SplitOption, roles);
                }

                model.Users = users;
                model.PagesCount = (int)Math.Ceiling(userInRoleCount / (decimal)countPerPage);
            }

            model.NewUser.IsNotAdded = invalidEmail != null ? true : false;
            model.NewUser.Email = invalidEmail ?? null;

            return this.View(model);
        }

        public async Task<IActionResult> EventsAll(string searchText, string searchCriteria, int page = 1, int countPerPage = PerPageDefaultValue)
        {
            var model = new EventsListAllViewModel<EventListViewModel>
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allEventsCount = await this.eventsService.GetAllEventsCountAsync(null, searchCriteria, searchText);
            if (allEventsCount > 0)
            {
                var events = await this.eventsService.GetAllPerPage<EventListViewModel>(page, countPerPage, null, searchCriteria, searchText);

                model.Events = events;
                model.PagesCount = (int)Math.Ceiling(allEventsCount / (decimal)countPerPage);
            }

            return this.View(model);

        }

        public async Task<IActionResult> GroupsAll(string searchText, string searchCriteria, int page = 1, int countPerPage = PerPageDefaultValue)
        {
            var model = new GroupsListAllViewModel
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allGroupsCount = await this.groupsService.GetAllGroupsCountAsync(null, searchCriteria, searchText);
            if (allGroupsCount > 0)
            {
                var groups = await this.groupsService.GetAllPerPageAsync<GroupsListViewModel>(page, countPerPage, null, searchCriteria, searchText);

                model.Groups = groups;
                model.PagesCount = (int)Math.Ceiling(allGroupsCount / (decimal)countPerPage);
            }

            return this.View(model);
        }

        public async Task<IActionResult> QuizzesAll(string searchText, string searchCriteria, int page = 1, int countPerPage = PerPageDefaultValue)
        {
            var model = new QuizzesAllListingViewModel
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            int quizzesCount = await this.quizzesService.GetAllQuizzesCountAsync(null, searchCriteria, searchText, null);

            if (quizzesCount > 0)
            {
                model.Quizzes = await this.quizzesService.GetAllPerPageAsync<QuizListViewModel>(page, countPerPage, null, searchCriteria, searchText, null);
                model.PagesCount = (int)Math.Ceiling(quizzesCount / (decimal)countPerPage);
            }

            return this.View(model);
        }

        public async Task<IActionResult> ResultsAll(string searchText, string searchCriteria, int page = 1, int countPerPage = PerPageDefaultValue)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> StudentsAll(string searchText, string searchCriteria, int page = 1, int countPerPage = PerPageDefaultValue)
        {
            var model = new StudentsAllViewModel
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchString = searchText,
                SearchType = searchCriteria,
            };

            var allStudentsCount = await this.userService.GetAllStudentsCountAsync(null, searchCriteria, searchText);

            if (allStudentsCount > 0)
            {
                model.Students = await this.userService.GetAllStudentsPerPageAsync<StudentsViewModel>(page, countPerPage, null, searchCriteria, searchText);
                model.PagesCount = (int)Math.Ceiling(allStudentsCount / (decimal)countPerPage);
            }

            return this.View(model);
        }
    }
}
