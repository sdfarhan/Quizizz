namespace Quizizz.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Quizizz.Data.Models;
    using Quizizz.Data.Validations;

    public class PasswordConfiguration : IEntityTypeConfiguration<Password>
    {
        public void Configure(EntityTypeBuilder<Password> password)
        {
            password.HasOne(p => p.Quiz)
                .WithOne(q => q.Password)
                .HasForeignKey<Quiz>(p => p.PasswordId);

            password.Property(p => p.Content)
                .HasMaxLength(DataValidation.Password.PasswordMaxLength);

            password.HasIndex(p => p.Content)
                .IsUnique();
        }
    }
}
