namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Data.Models;
    using Quizizz.Services.Events;
    using Quizizz.Services.Groups;
    using Quizizz.Services.Quizzes;
    using Quizizz.Web.ViewModels.Events;
    using Quizizz.Web.ViewModels.Groups;
    using Quizizz.Web.ViewModels.Quizzes;

    public class EventsController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEventsService eventsService;
        private readonly IGroupsService groupsService;
        private readonly IQuizzesService quizzesService;

        public EventsController(
            UserManager<ApplicationUser> userManager,
            IEventsService eventsService,
            IGroupsService groupsService,
            IQuizzesService quizzesService)
        {
            this.userManager = userManager;
            this.eventsService = eventsService;
            this.groupsService = groupsService;
            this.quizzesService = quizzesService;
        }

        public async Task<IActionResult> AllEventsCreatedByTeacher(string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new EventsListAllViewModel<EventListViewModel>
            {
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allEventsCount = await this.eventsService.GetAllEventsCountAsync(userId, searchCriteria, searchText);
            if (allEventsCount > 0)
            {
                var events = await this.eventsService.GetAllPerPage<EventListViewModel>(page, countPerPage, userId, searchCriteria, searchText);

                model.Events = events;
                model.PagesCount = (int)Math.Ceiling(allEventsCount / (decimal)countPerPage);
            }

            return this.View(model);
        }

        public IActionResult EventInput()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> EventInput(CreateEventInputViewModel model)
        {
            var timeErrorMessage = this.eventsService.GetTimeErrorMessage(model.ActiveFrom, model.ActiveTo, model.ActivationDate, model.TimeZone);
            if (timeErrorMessage != null)
            {
                model.Error = timeErrorMessage;
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);
            var eventId = await this.eventsService.CreateEventAsync(model.Name, model.ActivationDate, model.ActiveFrom, model.ActiveTo, userId);

            return this.RedirectToAction("AssignGroupsToEvent", new { id = eventId });
        }

        public async Task<IActionResult> AssignGroupsToEvent(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var groups = await this.groupsService.GetAllAsync<GroupAssignViewModel>(userId);
            var model = await this.eventsService.GetEventModelByIdAsync<EventWithGroupsViewModel>(id);
            model.Groups = groups;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignGroupsToEvent(EventWithGroupsViewModel model)
        {
            var groupIds = model.Groups.Where(x => x.IsAssigned).Select(x => x.Id).ToList();
            if (groupIds.Count == 0)
            {
                model.Error = true;
                return this.View(model);
            }

            await this.eventsService.AssignGroupsToEventAsync(groupIds, model.Id);
            return this.RedirectToAction("AssignQuizToEvent", new { id = model.Id });
        }

        public async Task<IActionResult> AssignQuizToEvent(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            IList<QuizAssignViewModel> quizzes;

            var isDashboardRequest = this.HttpContext.Session.GetString(GlobalConstants.DashboardRequest) != null;
            if (isDashboardRequest)
            {
                quizzes = await this.quizzesService.GetAllUnAssignedToEventAsync<QuizAssignViewModel>();
            }
            else
            {
                quizzes = await this.quizzesService.GetAllUnAssignedToEventAsync<QuizAssignViewModel>(userId);
            }

            var model = await this.eventsService.GetEventModelByIdAsync<EventWithQuizzesViewModel>(id);
            model.Quizzes = quizzes;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignQuizToEvent(EventWithQuizzesViewModel model)
        {
            var quizzes = model.Quizzes.Where(x => x.IsAssigned).ToList();

            if (quizzes.Count != 1)
            {
                model.Error = true;
                return this.View(model);
            }

            await this.eventsService.AssigQuizToEventAsync(model.Id, quizzes[0].Id, model.TimeZone);
            return this.RedirectToAction("EventDetails", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this.eventsService.DeleteAsync(id);
            return this.RedirectToAction("AllEventsCreatedByTeacher");
        }

        [HttpGet]
        public async Task<IActionResult> EventDetails(string id)
        {
            var groups = await this.groupsService.GetAllByEventIdAsync<GroupAssignViewModel>(id);
            var model = await this.eventsService.GetEventModelByIdAsync<EventDetailsViewModel>(id);
            var timeZoneIana = this.Request.Cookies[GlobalConstants.Coockies.TimeZoneIana];

            // TODO: The dateTime Converter
            model.Groups = groups;

            return this.View(model);
        }
    }
}
