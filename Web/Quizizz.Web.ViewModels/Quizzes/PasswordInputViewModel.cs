namespace Quizizz.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Quizizz.Web.ViewModels.Shared;

    public class PasswordInputViewModel
    {
        [Required]
        [StringLength(
            Modelvalidations.Password.MaxLength,
            ErrorMessage = Modelvalidations.Error.RangeMessage,
            MinimumLength = Modelvalidations.Password.MinLength)]
        public string Password { get; set; }

        public string Error { get; set; }
    }
}
