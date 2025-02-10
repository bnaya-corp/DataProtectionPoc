namespace DataProtection.Api.Data_Examples;

using DataProtection.Abstractions;
using Swashbuckle.AspNetCore.Filters;

public class PersonExample : IExamplesProvider<Person>
{
    public Person GetExamples()
    {
        return new Person
        {
            Id = 1,
            Name = "Example Laptop",
            Email = "bnaya@somewhere.com",
            SSN = "123-45-6789"
        };
    }
}