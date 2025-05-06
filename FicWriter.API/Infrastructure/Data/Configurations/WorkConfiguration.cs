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
            .HasMaxLength(500);

        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        builder.Property(w => w.DeletedAt)
            .IsRequired(false)
            .HasDefaultValue(null);

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

        builder.OwnsMany(w => w.Genres, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToTable("genres");
            
            ownedNavigationBuilder.HasKey("WorkId", "GenreType");
            
            ownedNavigationBuilder.WithOwner().HasForeignKey("WorkId");
            
            ownedNavigationBuilder.Property(g => g.GenreType)
                .HasConversion<string>()
                .IsRequired();
        });

        builder.OwnsMany(w => w.Tags, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToTable("tags");

            ownedNavigationBuilder.HasKey("WorkId", "Content");

            ownedNavigationBuilder.WithOwner().HasForeignKey("WorkId");

            ownedNavigationBuilder.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(128);
        });
    }
}
