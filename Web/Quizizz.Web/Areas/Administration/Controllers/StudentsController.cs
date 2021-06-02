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
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels.Students;

    public class StudentsController : AdministrationController
    {
        private const int DefaultCountPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public StudentsController(
            UserManager<ApplicationUser> userManager,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AllStudentsAddedByTeacher(string invalidEmail, string searchCriteria, string searchText, int page = 1, int countPerPage = DefaultCountPerPage)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new AllStudentsAddedByTeacherViewModel
            {
                NewStudent = new StudentInputViewModel(),
                CurrentPage = page,
                PagesCount = 0,
                SearchType = searchCriteria,
                SearchString = searchText,
            };

            var allStudentsAddedByTeacherCount = await this.usersService.GetAllStudentsCountAsync(userId, searchCriteria, searchText);
            if (allStudentsAddedByTeacherCount > 0)
            {
                var students = await this.usersService.GetAllStudentsPerPageAsync<StudentsViewModel>(page, countPerPage, userId, searchCriteria, searchText);
                model.Students = students;
                model.PagesCount = (int)Math.Ceiling(allStudentsAddedByTeacherCount / (decimal)countPerPage);
            }

            if (invalidEmail != null)
            {
                model.NewStudent.IsNotAdded = true;
                model.NewStudent.Email = invalidEmail;
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AllStudentsAddedByTeacher(AllStudentsAddedByTeacherViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var participantsIsAdded = await this.usersService.AddStudentAsync(model.NewStudent.Email, userId);

            if (!participantsIsAdded)
            {
                return this.RedirectToAction("AllStudentsAddedByTeacher", new { invalidEmail = model.NewStudent.Email });
            }

            return this.RedirectToAction("AllStudentsAddedByTeacher");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.usersService.DeleteFromTeacherListAsync(id, userId);
            return this.RedirectToAction("AllStudentsAddedByTeacher");
        }
    }
}
