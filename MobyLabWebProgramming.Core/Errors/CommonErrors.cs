using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);

    // Movie related errors
    public static ErrorMessage MovieNotFound => new(HttpStatusCode.NotFound, "The movie doesn't exist or was deleted!", ErrorCodes.EntityNotFound);
    public static ErrorMessage GenreNotFound => new(HttpStatusCode.NotFound, "One or more of the specified genres don't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage MovieAlreadyExists => new(HttpStatusCode.BadRequest, "A movie with this title and director already exists!", ErrorCodes.EntityAlreadyExists);
    
    // Rating related errors    
    public static ErrorMessage RatingNotFound => new(HttpStatusCode.NotFound, "The rating doesn't exist or was deleted!", ErrorCodes.EntityNotFound);
    public static ErrorMessage RatingAlreadyExists => new(HttpStatusCode.BadRequest, "You have already rated this movie!", ErrorCodes.EntityAlreadyExists);
    
    // Mail related errors
    public static ErrorMessage MailSendFailed => new(HttpStatusCode.ServiceUnavailable, "Failed to send email!", ErrorCodes.MailSendFailed);

    // Review related errors
    public static ErrorMessage ReviewNotFound => new(HttpStatusCode.NotFound, "The review doesn't exist or was deleted!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ReviewAlreadyExists => new(HttpStatusCode.BadRequest, "You have already reviewed this movie!", ErrorCodes.EntityAlreadyExists);
}
