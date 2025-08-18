
using Microsoft.AspNetCore.Mvc;
using Project.Application.Configuration.Validation;

namespace Project.Api.SeedWork
{
    /// <summary>
    /// Represents problem details for an <see cref="InvalidCommandException"/> error.
    /// </summary>
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandProblemDetails"/> class
        /// using the details from the specified <see cref="InvalidCommandException"/>.
        /// </summary>
        /// <param name="exception">The exception containing validation failure details.</param>
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = exception.Message;
            Status = StatusCodes.Status400BadRequest;
            Detail = exception.Details;
            Type = "https://domain/validation-error";
        }
    }
}