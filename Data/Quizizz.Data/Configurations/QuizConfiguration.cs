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

    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> quiz)
        {
            quiz.HasMany(q => q.Questions)
                .WithOne(qs => qs.Quiz)
                .HasForeignKey(q => q.QuizId);

            quiz.HasOne(q => q.Password)
                .WithOne(p => p.Quiz)
                .HasForeignKey<Quiz>(q => q.PasswordId);

            quiz.HasOne(q => q.Event)
                .WithOne(e => e.Quiz)
                .HasForeignKey<Event>(e => e.QuizId);

            quiz.Property(q => q.Name)
                .HasMaxLength(DataValidation.Quiz.NameMaxLength)
                .IsRequired();
        }
    }
}
