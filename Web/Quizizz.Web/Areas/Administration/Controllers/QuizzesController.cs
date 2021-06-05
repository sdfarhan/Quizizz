namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Data.Models;
    using Quizizz.Services.Categories;
    using Quizizz.Services.Questions;
    using Quizizz.Services.Quizzes;
    using Quizizz.Services.Users;
    using Quizizz.Web.Common;
    using Quizizz.Web.ViewModels.Categories;
    using Quizizz.Web.ViewModels.Questions;
    using Quizizz.Web.ViewModels.Quizzes;

    public class QuizzesController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private const int QuestionsPerPageDefaultValue = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;
        private readonly IQuestionsService questionsService;
        private readonly IQuizzesService quizzesService;

        public QuizzesController(
            UserManager<ApplicationUser> userManager,
            ICategoriesService categoriesService,
            IQuestionsService questionsService,
            IQuizzesService quizzesService)
        {
            this.userManager = userManager;
            this.categoriesService = categoriesService;
            this.questionsService = questionsService;
            this.quizzesService = quizzesService;
        }

        public async Task<IActionResult> AllQuizzesCreatedByTeacher(string categoryId, string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new QuizzesAllListingViewModel
            {
                Categories = await this.categoriesService.GetByCreatorIdAsync<CategorySimpleViewModel>(userId),
                CurrentCategory = categoryId == null ? null : await this.categoriesService.GetByIdAsync<CategorySimpleViewModel>(categoryId),
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allQuizzesCreatedByTeacherCount = await this.quizzesService.GetAllQuizzesCountAsync(userId, searchCriteria, searchText, categoryId);
            if (allQuizzesCreatedByTeacherCount > 0)
            {
                var quizzes = await this.quizzesService.GetAllPerPageAsync<QuizListViewModel>(page, countPerPage, userId, searchCriteria, searchText, categoryId);
                model.Quizzes = quizzes;
                model.PagesCount = (int)Math.Ceiling(allQuizzesCreatedByTeacherCount / (decimal)countPerPage);
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult DetailsInput()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> DetailsInput(InputQuizViewModel model)
        {
            var quizWithSamePasswordId = await this.quizzesService.GetQuizIdByPasswordAsync(model.Password);
            if (quizWithSamePasswordId != null)
            {
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);
            model.CreatorId = userId;
            model.PasswordIsValid = true;
            var quizId = await this.quizzesService.CreateQuizAsync(model.Name, model.Description, model.Timer, model.CreatorId, model.Password);
            this.HttpContext.Session.SetString(Constants.QuizSessionId, quizId);
            return this.RedirectToAction("QuestionsInput", "Questions");
        }

        public async Task<IActionResult> DeleteQuiz(string id)
        {
            await this.quizzesService.DeleteByIdAsync(id);
            return this.RedirectToAction(nameof(this.AllQuizzesCreatedByTeacher));
        }

        public async Task<IActionResult> Display(string id, int? page, int countPerPage = QuestionsPerPageDefaultValue)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                id = this.HttpContext.Session.GetString(Constants.QuizSessionId);
            }

            this.HttpContext.Session.SetString(Constants.QuizSessionId, id);

            if (page == null)
            {
                page = this.HttpContext.Session.GetInt32(Constants.PageToReturnTo) ?? 1;
            }

            var quizDetails = await this.quizzesService.GetQuizByIdAsync<QuizDetailsViewModel>(id);
            var model = new QuizDetailsPagingModel
            {
                Details = quizDetails,
                CurrentPage = (int)page,
                PagesCount = 0,
            };

            var questionCount = await this.questionsService.GetAllByQuizIdCountAsync(id);

            if (questionCount > 0)
            {
                model.Quetion = await this.questionsService.GetQuestionByQuizIdAndNumberAsync<QuestionViewModel>(id, (int)page);
                model.PagesCount = questionCount;
            }

            this.HttpContext.Session.SetInt32(Constants.PageToReturnTo, (int)page);
            return this.View(model);
        }

        public async Task<IActionResult> EditDetailsInput(string id)
        {
            var editmodel = await this.quizzesService.GetQuizByIdAsync<EditDetailsViewModel>(id);
            editmodel.PasswordIsValid = true;
            return this.View(editmodel);
        }
    }
}
