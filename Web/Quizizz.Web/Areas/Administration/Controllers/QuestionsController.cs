namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Services.Questions;
    using Quizizz.Web.Common;
    using Quizizz.Web.ViewModels.Questions;

    public class QuestionsController : AdministrationController
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        [HttpGet]
        public IActionResult QuestionsInput(string quizId)
        {
            if (quizId != null)
            {
                this.HttpContext.Session.SetString(Constants.QuizSessionId, quizId);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewQuestion(QuestionInputViewModel model)
        {
            var quizId = this.HttpContext.Session.GetString(Constants.QuizSessionId);
            var questionId = await this.questionsService.CreateQuestionAsync(quizId, model.Text);

            this.HttpContext.Session.SetString(Constants.CurrentQuestionId, questionId);
            return this.RedirectToAction("AnswerInput", "Answers");
        }

        [HttpGet]
        public async Task<IActionResult> EditQuestionInput(string id)
        {
            var model = await this.questionsService.GetByIdAsync<QuestionInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(QuestionInputModel model)
        {
            await this.questionsService.Update(model.Id, model.Text);
            var page = this.HttpContext.Session.GetInt32(Constants.PageToReturnTo);

            return this.RedirectToAction("Display", "Quizzes", new { page });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.questionsService.DeleteQuestionByIdAsync(id);

            return this.RedirectToAction("Display", "Quizzes", new { page = 1 });
        }
    }
}
