namespace Quizizz.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Quizizz.Common;
    using Quizizz.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorAndTeacherAuthorizationString)]
    [Area(GlobalConstants.Administration)]
    public class AdministrationController : BaseController
    {
    }
}
