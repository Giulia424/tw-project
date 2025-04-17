namespace MobyLabWebProgramming.Core.Constants;

public static class EmailTemplates
{
    public static string WelcomeEmail(string userName) => $@"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2>Welcome to MobyLab Movie Platform, {userName}!</h2>
                <p>We're excited to have you join our community. Start exploring movies now!</p>
                <p>Best regards,<br/>The MobyLab Team</p>
            </body>
        </html>";

    public static string MovieAddedToWatchlistNotification(string movieTitle) => $@"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2>{movieTitle} Added to Your Watchlist</h2>
                <p>The movie has been successfully added to your watchlist.</p>
                <p>Happy watching!</p>
                <p>Best regards,<br/>The MobyLab Team</p>
            </body>
        </html>";
} 