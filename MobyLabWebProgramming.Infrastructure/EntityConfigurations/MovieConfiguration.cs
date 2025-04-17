using MobyLabWebProgramming.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class MovieConfiguration: IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies"); // Set table name

        builder.HasKey(m => m.Id); // Primary Key

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Director)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.ReleaseDate)
            .IsRequired();

        builder.Property(m => m.Rating)
            .HasPrecision(3, 1); // Example: 8.5, 9.3

        builder.HasIndex(m => m.Title); // Create index for faster searches
    }
}