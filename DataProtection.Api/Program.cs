using DataProtection.Api.Data_Examples;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.ExampleFilters(); 
});
services.AddSwaggerExamplesFromAssemblyOf<PersonExample>();

builder.Logging.ClearProviders()
               .AddJsonConsole(c =>
               {
                   c.TimestampFormat = "[HH:mm:ss] ";
                   c.IncludeScopes = true;
                   c.JsonWriterOptions = new JsonWriterOptions
                   {
                       Indented = true
                   };
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
