using System;

namespace Project.Application.Configuration.Validation
{
    public class InvalidCommandException(string message, string details) : Exception(message)
    {
        public string Details { get; } = details;
    }
}