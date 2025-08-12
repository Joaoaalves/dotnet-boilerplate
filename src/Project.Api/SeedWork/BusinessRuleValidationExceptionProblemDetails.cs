using Microsoft.AspNetCore.Mvc;
using Project.Domain.SeedWork;

namespace Project.API.SeedWork
{
    /// <summary>
    /// Represents a standardized HTTP response for business rule validation exceptions.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="ProblemDetails"/> to provide consistent error responses
    /// when a <see cref="BusinessRuleValidationException"/> is thrown within the domain layer.
    /// It sets the HTTP status code to <c>409 Conflict</c> and includes details from the exception.
    /// </remarks>
    public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleValidationExceptionProblemDetails"/> class
        /// using the provided <see cref="BusinessRuleValidationException"/>.
        /// </summary>
        /// <param name="exception">The business rule validation exception containing error details.</param>
        /// <remarks>
        /// The <see cref="ProblemDetails.Title"/> is set to <c>"Business rule validation error"</c>,
        /// the <see cref="ProblemDetails.Status"/> is set to <c>409 Conflict</c>,
        /// and the <see cref="ProblemDetails.Detail"/> is populated with <see cref="BusinessRuleValidationException.Details"/>.
        /// </remarks>
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            Title = "Business rule validation error";
            Status = StatusCodes.Status409Conflict;
            Detail = exception.Details;
            Type = "https://somedomain/business-rule-validation-error";
        }
    }
}
