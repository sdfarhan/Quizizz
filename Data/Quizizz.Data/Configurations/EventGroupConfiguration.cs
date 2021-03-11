namespace Quizizz.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Quizizz.Data.Models;

    public class EventGroupConfiguration : IEntityTypeConfiguration<EventGroup>
    {
        public void Configure(EntityTypeBuilder<EventGroup> eventGroup)
        {
            eventGroup.HasKey(eg => new { eg.EventId, eg.GroupId });
        }
    }
}
