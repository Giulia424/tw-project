﻿using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Configurations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

/// <summary>
/// Inject the required service configuration from the application.json or environment variables.
/// </summary>
public class MailService(IOptions<MailConfiguration> mailConfiguration) : IMailService
{
    private readonly MailConfiguration _mailConfiguration = mailConfiguration.Value;


    public async Task<ServiceResponse> SendMail(string recipientEmail, string subject, string body, bool isHtmlBody = false, 
        string? senderTitle = null, CancellationToken cancellationToken = default)
    {
        if (!_mailConfiguration.MailEnable) // If you need only to test and not send emails you can set this variable to false, otherwise it will try to send the emails.
        {
            return ServiceResponse.ForSuccess();
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderTitle ?? _mailConfiguration.MailAddress, _mailConfiguration.MailAddress)); // Set the sender alias and sender's real address.
        message.To.Add(new MailboxAddress(recipientEmail, recipientEmail)); // Add the recipient mail address.
        message.Subject = subject; // Set the subject.
        message.Body = new TextPart(isHtmlBody ? "html" : "plain") { Text = body };  // Set the MIME type and email body.

        try
        {
            using var client = new SmtpClient(); // Create the SMTP client. Note that this object is disposable and as such need to use the keyword "using" to properly dispose the object after leaving the scope.
            await client.ConnectAsync(_mailConfiguration.MailHost, _mailConfiguration.MailPort, SecureSocketOptions.Auto, cancellationToken); // Connect to the mail host.
            client.AuthenticationMechanisms.Remove("XOAUTH2"); // Just to avoid issues with some clients this header is removed from the authentication request.
            await client.AuthenticateAsync(_mailConfiguration.MailUser, _mailConfiguration.MailPassword, cancellationToken); // Set the user and password for the email account.
            await client.SendAsync(message, cancellationToken); // Send the message.
            await client.DisconnectAsync(true, cancellationToken); // Disconnect the client from the host to save resources.
        }
        catch
        {
            return ServiceResponse.FromError(new(HttpStatusCode.ServiceUnavailable, "Mail couldn't be send!", ErrorCodes.MailSendFailed));
        }

        return ServiceResponse.ForSuccess();
    }


   

    // Sends a welcome email to a new user.
    public async Task<ServiceResponse> SendWelcomeEmailAsync(string recipientEmail, string userName,
        CancellationToken cancellationToken = default)
    {
        var subject = "Welcome to MobyLab Movie Platform!";
        var body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>Hi {userName},</h2>
                    <p>Welcome to <strong>MobyLab Movie Platform</strong>!</p>
                    <p>We're excited to have you join our movie-loving community.</p>
                    <p>Enjoy the show!<br/><em>The MobyLab Team</em></p>
                </body>
            </html>";

        return await SendMail(recipientEmail, subject, body, true, cancellationToken: cancellationToken);;
    }

    // Sends a notification when a new movie is added to the database.
    public async Task<ServiceResponse> SendMovieAddedNotificationAsync(string recipientEmail, string movieTitle,
        CancellationToken cancellationToken = default)
    {
        var subject = "New Movie Added";
        var body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>New Movie Added</h2>
                    <p>The movie <strong>{movieTitle}</strong> has been added to our platform.</p>
                    <p>Check it out now!</p>
                </body>
            </html>";

        return await SendMail(recipientEmail, subject, body, true, cancellationToken: cancellationToken);
    }

    public async Task<ServiceResponse> SendMovieAddedToWatchlistNotificationAsync(string recipientEmail, string movieTitle,
        CancellationToken cancellationToken = default)
    {
        var subject = "Movie Added to Watchlist";
        var body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>Movie Added to Watchlist</h2>
                    <p>The movie <strong>{movieTitle}</strong> has been added to your watchlist.</p>
                    <p>Happy watching!</p>
                </body>
            </html>";

        return await SendMail(recipientEmail, subject, body, true);
    }
}