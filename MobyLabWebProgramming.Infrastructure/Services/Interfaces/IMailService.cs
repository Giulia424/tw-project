using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IMailService
{
 
    public Task<ServiceResponse> SendMail(string recipientEmail, string subject, string body, bool isHtmlBody = false,
        string? senderTitle = null, CancellationToken cancellationToken = default);
    
    Task<ServiceResponse> SendWelcomeEmailAsync(string recipientEmail, string userName,
        CancellationToken cancellationToken = default);

   
    Task<ServiceResponse> SendMovieAddedNotificationAsync(string recipientEmail, string movieTitle,
        CancellationToken cancellationToken = default);
}