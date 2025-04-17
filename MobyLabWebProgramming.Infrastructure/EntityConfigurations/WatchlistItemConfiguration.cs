using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Configurations;

public class WatchlistItemConfiguration : IEntityTypeConfiguration<WatchlistItem>
{
    public void Configure(EntityTypeBuilder<WatchlistItem> builder)
    {
        builder.ToTable("WatchlistItems");
        
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Watched)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(w => w.CreatedAt)
            .IsRequired();
            
        builder.Property(w => w.UpdatedAt)
            .IsRequired();
            
        // Configure relationships
        builder.HasOne(w => w.User)
            .WithMany(u => u.WatchlistItems)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        builder.HasOne(w => w.Movie)
            .WithMany(m => m.WatchlistItems)
            .HasForeignKey(w => w.MovieId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        // Add indexes for better query performance
        builder.HasIndex(w => w.UserId);
        builder.HasIndex(w => w.MovieId);
    }
} 