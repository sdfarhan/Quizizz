namespace Quizizz.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Modelvalidations
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
    }
}
