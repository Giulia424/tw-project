using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(r => r.Content)
            .IsRequired()
            .HasMaxLength(2000);
            
        builder.Property(r => r.CreatedAt)
            .IsRequired();
            
        builder.Property(r => r.UpdatedAt)
            .IsRequired();
            
        // Configure relationships
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        builder.HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        // Add indexes for better query performance
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.MovieId);
        
        // Add a composite index for UserId and MovieId to ensure a user can only review a movie once
        builder.HasIndex(r => new { r.UserId, r.MovieId })
            .IsUnique();
    }
}
