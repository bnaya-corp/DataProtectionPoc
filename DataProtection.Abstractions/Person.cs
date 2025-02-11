namespace DataProtection.Abstractions;

public record Person(int Id, string Name, [PiiData] string Email, [SensitiveData] string SSN)
{
	public Person(): this(-1, string.Empty, string.Empty, string.Empty)
    {

	}
}
//public readonly record struct Person(int Id, string Name, [PiiData]string Email, [SensitiveData]string SSN);
