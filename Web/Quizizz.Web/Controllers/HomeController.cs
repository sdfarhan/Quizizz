namespace Quizizz.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Web.ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(GlobalConstants.AdministratorRoleName)
                    || this.User.IsInRole(GlobalConstants.TeacherRoleName))
                {
                    return this.Redirect("/Administration/Home/Index");
                }
                else
                {
                    return this.Redirect("/Students/Index");
                }
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string code)
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier, StatusCode = code });
        }

        public IActionResult SatusCode(string code)
        {
            return this.RedirectToAction("Error", new { code });
        }
    }
}
