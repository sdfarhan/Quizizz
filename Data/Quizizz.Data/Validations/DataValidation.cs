namespace Quizizz.Data.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class DataValidation
    {
        internal static class Answer
        {
            internal const int TextMaxLength = 1000;
        }

        internal static class Category
        {
            internal const int NameMaxLength = 50;
        }

        internal static class Event
        {
            internal const int NameMaxLength = 50;
        }

        internal static class Group
        {
            internal const int NameMaxLength = 50;
        }

        internal static class Password
        {
            internal const int PasswordMaxLength = 16;
        }

        internal static class Question
        {
            internal const int TextMaxLength = 1000;
        }

        internal static class Quiz
        {
            internal const int NameMaxLength = 50;
        }
    }
}
