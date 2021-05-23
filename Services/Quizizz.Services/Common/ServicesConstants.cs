namespace Quizizz.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ServicesConstants
    {
        public const string Name = "Name";
        public const string InvalidActivationDate = "Тhe activation date should not be earlier than today!";
        public const string InvalidStartingTime = "Invalid time for starting the event. Time for starting should not be earlier than current time!";
        public const string InvalidDurationOfActivity =
            "Invalid time for edning event. Time for ending should not be earlier than the time when event starts or the same!";
    }
}
