using DataProtection.Abstractions;
using DataProtection.Api.Data_Examples;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;
using DataProtection.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ReductionResponseFilter>(); // Add the response filter globally
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
});
services.AddSwaggerExamplesFromAssemblyOf<PersonExample>();

var logger = services.AddLogging();
var logging = builder.Logging;
logging.ClearProviders()
               .AddJsonConsole(c =>
               {
                   c.TimestampFormat = "[HH:mm:ss] ";
                   c.IncludeScopes = true;
                   c.JsonWriterOptions = new JsonWriterOptions
                   {
                       Indented = true
                   };
               })
               .EnableRedaction();


services.AddRedaction(x =>
{
    x.SetRedactor<ErasingRedactor>(DataTaxonomy.Sets.Sensitive);
    x.SetHmacRedactor(m =>
    {
        m.Key = Convert.ToBase64String("REPLACE_ME_WITH_REAL_SECRET_FROM_SECRET_MANAGER"u8);
        m.KeyId = 68;
    }, DataTaxonomy.Sets.Pii);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
