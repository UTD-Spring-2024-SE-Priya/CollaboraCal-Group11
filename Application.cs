using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

public static class Application
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            Console.WriteLine("IsDevelopment");
        }

        app.UseHttpsRedirection();

        // ADD ENDPOINTS
        // app.MapGet(), MapPost(), etc.

        app.MapGet("/", DefaultResponse).WithOpenApi();
        app.MapGet("/authenticate", AuthenticationTest).WithOpenApi();

        app.Run();
    }

    private static IResult DefaultResponse()
    {
        return TypedResults.Ok("Default Response");
    }

    private static IResult AuthenticationTest([FromHeader(Name = "Authorization")] string? auth)
    {
        if (auth == null) return TypedResults.Unauthorized();
        return TypedResults.Ok(new CalendarTestResponse2("Test String", auth));
    }

    /* MapGet() example using lambda expression

    app.MapGet("/weatherforecast", () =>
    {
        var forecast =  Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

    */

}

record CalendarTestResponse(string name, string email, int age);

record CalendarTestResponse2(string name, string requestEncoding);