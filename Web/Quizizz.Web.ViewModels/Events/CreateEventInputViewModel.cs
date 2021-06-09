namespace Quizizz.Web.ViewModels.Events
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Quizizz.Web.ViewModels.Shared;

    public class CreateEventInputViewModel
    {
        [Required]
        [StringLength(
            maximumLength: ModelValidations.Events.NameMaxLength,
            MinimumLength = ModelValidations.Events.NameMinLength,
            ErrorMessage = ModelValidations.Error.RangeMessage)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(
            ModelValidations.RegEx.Date,
            ErrorMessage = ModelValidations.Error.DateFormatMessage)]
        public string ActivationDate { get; set; }

        [Required]
        [RegularExpression(
            ModelValidations.RegEx.TimeActiveFrom,
            ErrorMessage = ModelValidations.Error.TimeActiveFromFormatMessage)]
        public string ActiveFrom { get; set; }

        [Required]
        [RegularExpression(
            ModelValidations.RegEx.TimeActiveTo,
            ErrorMessage = ModelValidations.Error.TimeActiveToMessage)]
        public string ActiveTo { get; set; }

        public string TimeZone { get; set; }

        public string Error { get; set; }
    }
}
