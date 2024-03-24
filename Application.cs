using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

public static class Application
{

    public static AuthenticationSystem AuthenticationSystem { get; }
    public static DatabaseController Database { get; }



    static Application()
    {
        AuthenticationSystem = new AuthenticationSystem();
        Database = new DatabaseController();
    }

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
        app.MapGet("/authenticate", AuthenticationTest).WithName("Authenticate").WithOpenApi();
        app.MapPost("/login", Login);


        app.Run();
    }

    private static IResult Login([FromHeader(Name = "Username")] string? username, [FromHeader(Name = "Password")] string? password)
    {
        if (username == null || password == null) return TypedResults.BadRequest("Missing 'Username' and/or 'Password' headers");
        var result = AuthenticationSystem.Login(username, password);
        if (result == null) return TypedResults.Unauthorized();
        return TypedResults.Ok(new LoginResponse(result, username));
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

    private static IResult CreateUser([FromHeader(Name = "Username")] string? username, [FromHeader(Name = "Password")] string? password)
    {
        if (username == null || password == null) return TypedResults.BadRequest();
        return TypedResults.Ok(Database.createUser(username, password));
    }

}

record LoginResponse(string authentication, string username);

record CalendarTestResponse(string name, string email, int age);

record CalendarTestResponse2(string name, string requestEncoding);