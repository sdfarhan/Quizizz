namespace Quizizz.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Quizizz";

        public const string Administration = "Administration";

        public const string AdministratorRoleName = "Administrator";

        public const string TeacherRoleName = "Teacher";

        public const string AdministratorAndTeacherAuthorizationString = "Administrator, Teacher";

        public const string AdminLayout = "_LayoutAdmin";

        public const string PageToReturnTo = "Page";

        public static class Coockies
        {
            public const string TimeZoneIana = "timeZoneIana";
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
