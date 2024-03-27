using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class Application
{
    public static AuthenticationSystem AuthenticationSystem { get; }
    public static DatabaseController Database { get; }
    public static AccountController Accounts { get; }

    static Application()
    {
        AuthenticationSystem = new AuthenticationSystem();
        Database = new DatabaseController();
        Accounts = new AccountController();
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
        app.MapPost("/login", Login).WithName("Login").WithOpenApi();
        app.MapPost("/newuser", CreateUser).WithName("New User").WithOpenApi();
        app.MapPost("/changename", ChangeName).WithName("Update Name").WithOpenApi();

        app.Run();
    }

    private static IResult DefaultResponse()
    {
        return TypedResults.Ok("Default Response");
    }

    private static IResult Login(
        [FromHeader(Name = "EMail")] string? email, 
        [FromHeader(Name = "Password")] string? password)
    {
        if (email == null || password == null) return TypedResults.BadRequest("Missing 'EMail' and/or 'Password' headers");
        var result = AuthenticationSystem.Login(email, password);
        if (result == null) return TypedResults.Unauthorized();
        return TypedResults.Ok(new LoginResponse(result, email));
    }

    private static IResult CreateUser(
        [FromHeader(Name = "EMail")] string? email, 
        [FromHeader(Name = "Password")] string? password,
        [FromHeader(Name = "Name")] string? name)
    {
        if (email == null || name == null || password == null) return TypedResults.BadRequest();
        return TypedResults.Ok(Accounts.CreateUser(email, name, password));
    }

    private static IResult ChangeName(
        [FromHeader(Name = "Email")] string? email,
        [FromHeader(Name = "Authentication")] string? authentication,
        [FromHeader(Name = "Name")] string? newName
    )
    {
        if (email == null || newName == null || authentication == null) return TypedResults.BadRequest();
        bool success = Accounts.ChangeName(email, authentication, newName);
        if (success) return TypedResults.Ok();
        else return TypedResults.Unauthorized();
    }

}

record LoginResponse(string authentication, string email);
