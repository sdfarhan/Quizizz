namespace Quizizz.Data.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Quizizz.Data.Models;

    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasMany(g => g.StudentsGroups)
                .WithOne(sg => sg.Group)
                .HasForeignKey(sg => sg.GroupId);

            builder.HasMany(g => g.EventsGroups)
                .WithOne(eg => eg.Group)
                .HasForeignKey(eg => eg.GroupId);

            builder.Property(g => g.Name)
                .HasMaxLength(DataValidation.Group.NameMaxLength)
                .IsRequired();
        }
    }
}
