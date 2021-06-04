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
    using Quizizz.Web.Common;
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
        public async Task<IActionResult> AddNewAnswer(AnswerViewModel model)
        {
            var questionId = this.HttpContext.Session.GetString(Constants.CurrentQuestionId);
            await this.answerService.CreateAnswerAsync(model.SanitizedText, model.IsRightAnswer, questionId);
            return this.RedirectToAction("AnswerInput");
        }

        [HttpGet]
        public IActionResult AppendAnswerInput(string id)
        {
            var model = new AnswerViewModel
            {
                QuestionId = id,
            };

            return this.View(model);
        }
    }
}
