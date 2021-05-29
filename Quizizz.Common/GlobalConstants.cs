namespace Quizizz.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Quizizz";

        public const string Administration = "Administration";

        public const string AdministratorRoleName = "Administrator";

        public const string AdministratorAndTeacherAuthorizationString = "Administrator, Teacher";

        public const string AdminLayout = "_LayoutAdmin";

        public const string PageToReturnTo = "Page";

        public const string TeacherRoleName = "Teacher";

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
    }
}
