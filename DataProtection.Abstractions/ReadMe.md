# Resources

- [Redacting sensitive data in logs with Microsoft​.Extensions​.Compliance​.Redaction](https://andrewlock.net/redacting-sensitive-data-with-microsoft-extensions-compliance/)
- [The New Data Protection Features of .NET 8 (GDPR)](https://www.youtube.com/watch?v=rK3-tO7K6i8&t=724s)

# ⚠️ Hazards

- Not working with readonly record struct 
- Not Working with 

```cs
[LoggerMessage(LogLevel.Information, "Person {person}")]
public static partial void LogPerson(this ILogger logger, Person person);
```

Only working with `[LogProperties]`

```cs
[LoggerMessage(LogLevel.Information, "Person")]
public static partial void LogPerson(this ILogger logger, [LogProperties]Person person);
```
