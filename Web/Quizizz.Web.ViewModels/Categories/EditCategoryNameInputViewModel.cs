namespace Quizizz.Web.ViewModels.Categories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Quizizz.Data.Models;
    using Quizizz.Services.Mapping;
    using Quizizz.Web.ViewModels.Shared;

    public class EditCategoryNameInputViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(
            ModelValidations.Categories.NameMaxLength,
            ErrorMessage = ModelValidations.Error.RangeMessage,
            MinimumLength = ModelValidations.Categories.NameMinLength)]
        public string Name { get; set; }
    }
}
