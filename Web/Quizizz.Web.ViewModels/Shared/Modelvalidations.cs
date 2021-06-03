namespace Quizizz.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ModelValidations
    {
        public static class Answer
        {
            public const int TextMinLength = 1;

            public const int TextMaxLength = 1000;
        }

        public static class Error
        {
            public const string RangeMessage = "The {0} must be at least {2} and at max {1} characters long.";
        }

        public static class Password
        {
            public const int MaxLength = 16;

            public const int MinLength = 8;
        }

        public static class Questions
        {
            public const int TextMaxLength = 1000;

            public const int TextMinLength = 3;
        }

        public static class Quizzes
        {
            public const int NameMaxLength = 1000;

            public const int NameMinLength = 3;
        }
    }
}
