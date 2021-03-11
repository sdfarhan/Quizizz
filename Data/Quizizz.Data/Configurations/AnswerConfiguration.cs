namespace Quizizz.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Quizizz.Data.Models;
    using Quizizz.Data.Validations;

    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> answer)
        {
            answer.Property(a => a.Text)
                .HasMaxLength(DataValidation.Answer.TextMaxLength)
                .IsRequired();
            answer.Property(a => a.IsCorrectAnswer)
                .HasDefaultValue(false);
        }
    }
}
