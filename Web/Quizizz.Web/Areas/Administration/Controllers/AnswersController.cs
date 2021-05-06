namespace Quizizz.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Services.Answers;
    using Quizizz.Web.ViewModels.Answers;

    public class AnswersController : AdministrationController
    {
        private readonly IAnswersService answerService;

        public AnswersController(IAnswersService answersService)
        {
            this.answerService = answersService;
        }

        [HttpGet]
        public IActionResult AnswerInput()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AppendAnswerInput(string id)
        {
            var model = new AnswerViewModel() { QuestionId = id };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult ApendAnswerInput(string id)
        {
            var model = new AnswerViewModel() { QuestionId = id };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AppendNewAnswer(AnswerViewModel model)
        {
            await this.answerService.CreateAnswerAsync(model.Text, model.IsRightAnswer, model.QuestionId);
            var page = this.HttpContext.Session.GetInt32(GlobalConstants.PageToReturnTo);

            return this.RedirectToAction("Display", "Quizzes", new { page });
        }

        [HttpGet]
        public async Task<IActionResult> EditAnswerInput(string id)
        {
            var model = await this.answerService.GetByIdAsync<AnswerViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AnswerViewModel model)
        {
            await this.answerService.UpdateAsync(model.Id, model.Text, model.IsRightAnswer);
            var page = this.HttpContext.Session.GetInt32(GlobalConstants.PageToReturnTo);
            return this.RedirectToAction("Display", "Quizzes", new { page });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this.answerService.DeleteAsync(id);
            var page = this.HttpContext.Session.GetInt32(GlobalConstants.PageToReturnTo);
            return this.RedirectToAction("Display", "Quizzes", new { page });
        }
    }
}
