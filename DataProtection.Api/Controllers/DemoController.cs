using DataProtection.Abstractions;
using DataProtection.Api.Data_Examples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Concurrent;

namespace DataProtection.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, Person> _people = new();

    private readonly ILogger<DemoController> _logger;

    public DemoController(ILogger<DemoController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [SwaggerRequestExample(typeof(Person), typeof(PersonExample))]
    public async Task<Person> PostAsync([FromBody]Person person)
    {
        await Task.Delay(400);
        _people.AddOrUpdate(person.Id, person, (_, _) => person);
        _logger.LogPerson(person);
        return person;
    }

    [HttpGet]
    public async Task<Person[]> GetAsync()
    {
        await Task.Delay(400);
        return _people.Values.ToArray();
    }
}
