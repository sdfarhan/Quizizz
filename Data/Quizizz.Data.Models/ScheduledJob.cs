// ReSharper disable VirtualMemberCallInConstructor
namespace Quizizz.Data.Models
{
    using Quizizz.Data.Common.Models;

    public class ScheduledJob : BaseDeletableModel<string>
    {
        public string JobId { get; set; }

        public bool IsActivationJob { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
