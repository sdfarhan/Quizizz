namespace Quizizz.Web.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Quizizz.Web.ViewModels.Shared;

    public class EditGroupNameInputViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(
            maximumLength: ModelValidations.Groups.NameMaxLength,
            MinimumLength = ModelValidations.Groups.NameMinLength,
            ErrorMessage = ModelValidations.Error.RangeMessage)]
        public string Name { get; set; }
    }
}
