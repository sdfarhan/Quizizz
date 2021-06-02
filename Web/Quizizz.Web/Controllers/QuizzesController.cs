namespace Quizizz.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Models;
    using Quizizz.Services.Quizzes;
    using Quizizz.Web.Common;
    using Quizizz.Web.ViewModels.Quizzes;

    public class QuizzesController : Controller
    {
        private readonly IQuizzesService quizzesService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuizzesController(
            IQuizzesService quizzesService,
            UserManager<ApplicationUser> userManager)
        {
            this.quizzesService = quizzesService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Start(string password, string id)
        {
            id ??= await this.quizzesService.GetQuizIdByPasswordAsync(password);

            var user = await this.userManager.GetUserAsync(this.User);
            var roles = await this.userManager.GetRolesAsync(user);

            this.ViewData["Area"] = roles.Count > 0 ? Constants.AdminArea : string.Empty;
            var quizModel = await this.quizzesService.GetQuizByIdAsync<AttemptedQuizViewModel>(id);

            foreach (var question in quizModel.Questions)
            {
            }

            return this.NoContent();
        }
    }
}
