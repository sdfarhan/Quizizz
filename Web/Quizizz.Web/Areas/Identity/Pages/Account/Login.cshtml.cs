namespace Quizizz.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Quizizz.Common;
    using Quizizz.Data.Models;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private ILogger<LoginModel> logger;

        public LoginModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl ??= this.Url.Content("~/");

            // clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await this.userManager.FindByEmailAsync(this.Input.Email);
                    var roles = await this.userManager.GetRolesAsync(user);

                    if (roles.Count > 0)
                    {
                        returnUrl = this.Url.Content("~/Administration/Home/index");
                    }
                    else
                    {
                        returnUrl = this.Url.Content("~/Students/Index");
                    }

                    this.logger.LogInformation($"{user.UserName} logged in.");
                    var option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append(GlobalConstants.Coockies.TimeZoneIana, this.Input.TimeZone, option);
                    return this.LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning($"user account locked out.");
                    return this.LocalRedirect("./lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login Attempt.");
                    return this.Page();
                }
            }

            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remeber me?")]
            public bool RememberMe { get; set; }

            public string TimeZone { get; set; }
        }

    }
}
