using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FicWriter.API.Infrastructure.Data.Configurations;

public class DraftConfiguration : IEntityTypeConfiguration<Draft>
{
    public void Configure(EntityTypeBuilder<Draft> builder)
    {
        builder.Metadata.SetTableName("drafts");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();

        builder.Property(d => d.Order)
            .IsRequired();

        builder.HasOne(d => d.Work)
            .WithMany(w => w.Drafts)
            .HasForeignKey(d => d.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
