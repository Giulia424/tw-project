using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;


namespace MobyLabWebProgramming.Infrastructure.Database;

/// <summary>
/// This is the database context used to connect with the database and links the ORM, Entity Framework, with it.
/// </summary>
public sealed class WebAppDatabaseContext : DbContext
{
    public WebAppDatabaseContext(DbContextOptions<WebAppDatabaseContext> options, bool migrate = true) : base(options)
    {
        if (migrate)
        {
            Database.Migrate();
        }
    }

    /// <summary>
    /// Here additional configuration for the ORM is performed.
    /// </summary>
    /// 
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserFile> UserFiles { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<MovieGenre> MovieGenres { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;
    public DbSet<WatchlistItem> WatchlistItems { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("unaccent")
            .ApplyAllConfigurationsFromCurrentAssembly(); // Here all the classes that contain implement IEntityTypeConfiguration<T> are searched at runtime
                                                          // such that each entity that needs to be mapped to the database tables is configured accordingly.
        
        // Configure many-to-many relationship between Movie and Genre
        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });
            
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);
            
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);
            
        // Configure Review relationships
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);
            
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId);
            
        // Configure Rating relationships
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId);
            
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Ratings)
            .HasForeignKey(r => r.MovieId);
            
        // Configure WatchlistItem relationships
        modelBuilder.Entity<WatchlistItem>()
            .HasOne(w => w.User)
            .WithMany(u => u.WatchlistItems)
            .HasForeignKey(w => w.UserId);
            
        modelBuilder.Entity<WatchlistItem>()
            .HasOne(w => w.Movie)
            .WithMany(m => m.WatchlistItems)
            .HasForeignKey(w => w.MovieId);
            
        // Existing UserFile configuration
        modelBuilder.Entity<UserFile>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UserFiles)
            .HasForeignKey(uf => uf.UserId);                                                   
                                                          
                                                          
                                                          
                                                          
                                                          
    }
}