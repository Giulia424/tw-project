using Microsoft.Extensions.Options;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Repositories;
using MobyLabWebProgramming.Infrastructure.Configurations;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Repositories;
using MobyLabWebProgramming.Infrastructure.Repositories.Implementation;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using IGenreService = MobyLabWebProgramming.Infrastructure.Services.Interfaces.IGenreService;
using IMovieService = MobyLabWebProgramming.Infrastructure.Services.Interfaces.IMovieService;

// TODO: ADD ANOTHER MAILTRAP ACTION
//Controllers: Handle HTTP requests/responses
// Services: Implement business logic
// Repositories: Handle data access
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IWatchlistItemRepository, WatchlistItemRepository>();
builder.Services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();

// Register services
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

//builder.Services.AddScoped<IReviewService, ReviewService>();
//builder.Services.AddScoped<IWatchlistService, WatchlistService>();

// Configure the application
builder.AddCorsConfiguration()
    .AddRepository()
    .AddAuthorizationWithSwagger("MobyLab Web App")
    .AddServices()
    .UseLogger()
    .AddWorkers()
    .AddApi();

var app = builder.Build();

app.ConfigureApplication();
app.Run();
