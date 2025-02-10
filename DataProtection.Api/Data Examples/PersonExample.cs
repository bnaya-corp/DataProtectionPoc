namespace DataProtection.Api.Data_Examples;

using DataProtection.Abstractions;
using Swashbuckle.AspNetCore.Filters;

public class PersonExample : IExamplesProvider<Person>
{
    public Person GetExamples()
    {
        return new Person(1, "Bnaya", "bnaya@somewhere.com", "123-45-6789");
    }
}