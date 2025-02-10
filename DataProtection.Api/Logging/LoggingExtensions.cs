using DataProtection.Abstractions;

namespace DataProtection.Api;

public static partial class LoggingExtensions
{

    [LoggerMessage(LogLevel.Information, "Person")]
    public static partial void LogPerson(this ILogger logger, [LogProperties]Person person);
}
