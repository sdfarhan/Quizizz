namespace Quizizz.Common
{
    public static class GlobalConstants
    {
        public const string Administration = "Administration";

        public const string Administrator = "Administrator";

        public const string AdministratorRoleName = "Administrator";

        public const string AdministratorAndTeacherAuthorizationString = "Administrator, Teacher";

        public const string AdminLayout = "_LayoutAdmin";

        public const int CookieTimeOut = 1;

        public const string DashboardRequest = "DashboardRequest";
        
        public const string Empty = "empty";

        public const string PageToReturnTo = "Page";

        public const string Teacher = "Teacher";

        public const string TeacherRoleName = "Teacher";

        public const string SplitOption = ", ";

        public const string SystemName = "Quizizz";

        public static class Coockies
        {
            public const string TimeZoneIana = "timeZoneIana";
        }

        public static class DataSeeding
        {
            public const string Password = "123456";

            public const string AdminEmail = "admin@admin.com";

            public const string AdminName = "Admin";

            public const string TeacherName = "Teacher";

            public const string TeacherEmail = "teacher@teacher.com";

            public const string StudentName = "Student";

            public const string StudentEmail = "student@student.com";
        }

        public static class EmailSender
        {
            public const string EventInvitationSubject = "Event Inviatition";

            public const string ConfirmEmailSubject = "Confirm your email";

            public const string SenderEmail = "quizizzproject@gmail.com";

            public const string SenderName = "Quizizz Team";

            public const string StringToReplace = "{password}";
        }

        public static class ErrorMessages
        {
            public const string EmptyEmailField = "You should enter email if you want to add somebody to role";
        }
    }
}
