using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FicWriter.API.Infrastructure.Data.Configurations;

public class WorkConfiguration : IEntityTypeConfiguration<Work>
{
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        builder.Property(w => w.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(w => w.IsArchived)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(w => w.Drafts)
            .WithOne(d => d.Work)
            .HasForeignKey(d => d.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
