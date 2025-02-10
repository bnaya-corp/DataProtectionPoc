namespace DataProtection.Abstractions;

public record Person(int Id, string Name, [PiiData]string Email, [SensitiveData]string SSN);
//public readonly record struct Person(int Id, string Name, [PiiData]string Email, [SensitiveData]string SSN);
