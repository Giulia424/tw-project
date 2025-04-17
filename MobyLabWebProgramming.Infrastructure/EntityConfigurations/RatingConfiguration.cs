using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Value)
            .IsRequired();
            
        builder.Property(r => r.CreatedAt)
            .IsRequired();
            
        builder.Property(r => r.UpdatedAt)
            .IsRequired();
            
        // Configure relationships
        builder.HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        builder.HasOne(r => r.Movie)
            .WithMany(m => m.Ratings)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
        // Add indexes for better query performance
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.MovieId);
        
        // Add a composite index for UserId and MovieId to ensure a user can only rate a movie once
        builder.HasIndex(r => new { r.UserId, r.MovieId })
            .IsUnique();
            
        // Add a check constraint to ensure rating value is between 1 and 10
        builder.HasCheckConstraint("CK_Ratings_Value", "Value >= 1 AND Value <= 10");
    }
}
