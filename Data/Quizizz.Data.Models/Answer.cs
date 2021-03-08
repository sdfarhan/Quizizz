﻿// ReSharper disable VirtualMemberCallInConstructor
namespace Quizizz.Data.Models
{
    using System;

    using Quizizz.Data.Common.Models;

    public class Answer : BaseDeletableModel<string>
    {
        public Answer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }

        public bool? IsCorrectAnswer { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
