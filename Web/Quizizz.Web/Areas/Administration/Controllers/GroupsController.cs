namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Common.Enumerations;
    using Quizizz.Data.Models;
    using Quizizz.Services.Events;
    using Quizizz.Services.Groups;
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels.Events;
    using Quizizz.Web.ViewModels.Groups;
    using Quizizz.Web.ViewModels.Students;

    public class GroupsController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEventsService eventsService;
        private readonly IGroupsService groupsService;
        private readonly IUsersService usersService;

        public GroupsController(
            UserManager<ApplicationUser> userManager,
            IEventsService eventsService,
            IGroupsService groupsService,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.eventsService = eventsService;
            this.groupsService = groupsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> AllGroupsCreatedByTeacher(string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
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

        public async Task<IActionResult> AssignEvent(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var events = await this.eventsService.GetAllFiteredByStatusAndGroupAsync<EventsAssignViewModel>(Status.Ended, id, userId);
            var model = await this.groupsService.GetGroupModelAsync<GroupWithEventsViewModel>(id);
            model.Events = events;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignEvent(GroupWithEventsViewModel model)
        {
            var eventsIds = model.Events.Where(x => x.IsAssigned).Select(x => x.Id).ToList();
            if (eventsIds.Count == 0)
            {
                model.Error = true;
                return this.View(model);
            }

            await this.groupsService.AssignEventsToGroupAsync(model.Id, eventsIds);
            return this.RedirectToAction("AssignStudents", new { id = model.Id });
        }

        public async Task<IActionResult> AssignStudents(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var students = await this.usersService.GetAllStudentsAsync<StudentsViewModel>(userId);
            var model = new GroupWithStudentsViewModel
            {
                Id = id,
                Students = students,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignStudents(GroupWithStudentsViewModel model)
        {
            var studentsIds = model.Students.Where(x => x.IsAssigned).Select(x => x.Id).ToList();

            if (studentsIds.Count == 0)
            {
                model.Error = true;
                return this.View(model);
            }

            await this.groupsService.AssignStudentsToGroupAsync(model.Id, studentsIds);
            return this.RedirectToAction("GroupDetails", new { id = model.Id });
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupInputViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var groupId = await this.groupsService.CreateGroupAsync(model.Name, userId);

            return this.RedirectToAction("AssignEvent", new { id = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.groupsService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.AllGroupsCreatedByTeacher), "Groups");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            return this.RedirectToAction("GroupDetails", new { id });
        }

        public IActionResult EditName(string id, string name)
        {
            var model = new EditGroupNameInputViewModel
            {
                Id = id,
                Name = name,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditName(EditGroupNameInputViewModel model)
        {
            await this.groupsService.UpdateNameAsync(model.Id, model.Name);
            return this.RedirectToAction("GroupDetails", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GroupDetails(string id)
        {
            var events = await this.eventsService.GetAllByGroupIdAsync<EventsAssignViewModel>(id);
            var students = await this.usersService.GetAllByGroupIdAsync<StudentsViewModel>(id);
            var model = await this.groupsService.GetGroupModelAsync<GroupDetailsViewModel>(id);

            model.Events = events;
            model.Students = students;

            return this.View(model);
        }
    }
}
