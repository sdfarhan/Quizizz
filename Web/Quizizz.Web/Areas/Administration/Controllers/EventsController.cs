namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Models;
    using Quizizz.Services.Events;
    using Quizizz.Web.ViewModels.Events;

    public class EventsController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEventsService eventsService;

        public EventsController(
            UserManager<ApplicationUser> userManager,
            IEventsService eventsService)
        {
            this.userManager = userManager;
            this.eventsService = eventsService;
        }

        public async Task<IActionResult> AllEventsCreatedByTeacher(string searchCriteria, string searchText, int page = 0, int countPerPage = DefaultCountPerPage)
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
    }
}
