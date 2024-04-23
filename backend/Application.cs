using System;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CollaboraCal.JsonRequests;
using Microsoft.EntityFrameworkCore.Storage.Json;
using SQLitePCL;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace CollaboraCal
{
    public static class Application
    {
        public static ActiveSessionHandler Sessions { get; }
        public static DatabaseController Database { get; }
        public static AccountController Accounts { get; }
        public static CalendarManager CalendarManager { get; }

        static Application()
        {
            Sessions = new ActiveSessionHandler();
            Database = new DatabaseController();
            Accounts = new AccountController();
            CalendarManager = new CalendarManager();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
                options.AddPolicy(name: "AllowReactLocalServer", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                    //policy.WithOrigins("https://localhost:3000", "http://localhost:3000", "localhost:3000");
                }
            ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                Console.WriteLine("IsDevelopment");
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactLocalServer");

            // Default
            app.MapGet("/", DefaultResponse).WithOpenApi();

            // User
            app.MapPost("/login", Login).WithName("Login").WithOpenApi();
            app.MapPost("/logout", Logout).WithName("Logout").WithOpenApi();
            app.MapPost("/newuser", CreateUser).WithName("New User").WithOpenApi();
            app.MapPost("/changename", ChangeName).WithName("Update Name").WithOpenApi();
            app.MapPost("/resetpassword", ChangePassword).WithName("Reset Password").WithOpenApi();
            app.MapGet("/validate", CheckAuthenticationValidity).WithName("Validate Authentication").WithOpenApi();

            // Calendar
            app.MapPost("/newcalendar", CreateCalendar).WithName("New Calendar").WithOpenApi();
            app.MapPost("/newevent", CreateEvent).WithName("Create Event").WithOpenApi();
            app.MapPost("/deleteevent", DeleteEvent).WithName("Delete Event").WithOpenApi();
            app.MapPost("/deletecalendar", DeleteCalendar).WithName("Delete Calendar").WithOpenApi();

            app.MapGet("/getallcalendar", GetAllCalendars).WithName("Get All Calendars From User").WithOpenApi();
            app.MapPost("/getevents", GetEventsDuringTimeFrame).WithName("Get All Events In Calendar Within Timeframe").WithOpenApi();
            app.MapPost("/share", ShareCalendar).WithName("Share").WithOpenApi();
            // Should this be a POST request?? NO! Is it the only way to make the JSON Body arguments work with this little time??

            app.Run();
        }

        private static IResult DefaultResponse()
        {
            return TypedResults.Ok("Default Response");
        }

        private static IResult Login(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Password")] string? password)
        {
            if (email == null || password == null)
                return TypedResults.BadRequest("Missing 'Email' and/or 'Password' headers");
            var result = Sessions.Login(email, password);
            if (result == null) return TypedResults.Unauthorized();
            return TypedResults.Ok(new LoginResponse(result, email));
        }

        private static IResult Logout(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication
        )
        {
            if (email == null || authentication == null)
                return TypedResults.BadRequest("Missing 'Email' and/or 'Password' headers");
            if (!Sessions.ValidateAuthentication(email, authentication))
                return TypedResults.Unauthorized();

            _ = Sessions.Logout(email);
            return TypedResults.Ok();
        }

        private static IResult CreateUser(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Password")] string? password,
            //            [FromHeader(Name = "ConfirmPassword")] string? confpassword,
            [FromBody] string? name
        )
        {
            if (email == null || name == null || password == null) return TypedResults.BadRequest();
            Accounts.CreateUser(email, name, password, password);
            return Login(email, password);
        }

        private static IResult ChangeName(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? newName
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest("Authentication headers missing");
            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            if (newName == null) return TypedResults.BadRequest("Missing body");
            bool success = Accounts.ChangeName(email, authentication, newName);
            if (success) return TypedResults.Ok();
            else return TypedResults.Unauthorized();
        }

        private static IResult CheckAuthenticationValidity(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            bool success = Sessions.ValidateAuthentication(email, authentication);
            if (success) return TypedResults.Ok();
            else return TypedResults.Unauthorized();
        }

        private static IResult CreateCalendar(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? jsonBody
        )
        {
            if (email == null || authentication == null)
                return TypedResults.BadRequest("Missing user or authentication");

            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            if (jsonBody == null) return TypedResults.BadRequest("Missing body");
            var data = JsonConvert.DeserializeObject<NewCalendarData>(jsonBody);
            if (data == null) return TypedResults.BadRequest("Malformed body");

            Calendar? calendar = CalendarManager.CreateCalendar(email, data);
            if (calendar != null)
            {
                var clr = new CalendarListResponse(calendar.ID, calendar.Name, calendar.Description);
                string asJson = JsonConvert.SerializeObject(clr);
                return TypedResults.Ok(asJson);
            }
            else
                return TypedResults.Forbid();
        }

        private static IResult DeleteCalendar(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] int? cID
        )
        {
            if (email == null || authentication == null)
                return TypedResults.BadRequest("Missing user or authentication");

            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            User? user = Database.GetHeavyUserFromEmail(email);
            if (user == null) return TypedResults.NoContent();

            if (cID == null) return TypedResults.BadRequest();
            int calendarID = cID.Value;

            Calendar? cal = Database.GetHeavyCalendar(calendarID);
            if (cal == null) return TypedResults.BadRequest();

            if (cal.Users?.First().ID == user.ID)
            {
                CalendarManager.DeleteCalendar(calendarID);
            }
            else
            {
                user.Calendars?.RemoveAll(a => a.ID == calendarID);
                Database.Context.SaveChanges();
            }

            return TypedResults.Ok();
        }

        private static IResult CreateEvent(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? jsonBody
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (jsonBody == null) return TypedResults.BadRequest();

            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            var body = JsonConvert.DeserializeObject<NewEventData>(jsonBody);
            if (body == null) return TypedResults.BadRequest();

            bool success = CalendarManager.CreateEvent(email, body);
            if (success)
                return TypedResults.Ok();
            else
                return TypedResults.Forbid();
        }

        private static IResult DeleteEvent(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] int? eventID
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (eventID == null) return TypedResults.BadRequest();

            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            bool success = CalendarManager.DeleteEvent(email, eventID.Value);

            if (success)
                return TypedResults.Ok();
            else
                return TypedResults.Unauthorized();
        }

        private static IResult GetAllCalendars(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (!Sessions.ValidateAuthentication(email, authentication))
            {
                return TypedResults.Unauthorized();
            }

            List<Calendar>? calendars = Database.GetLightAllCalendars(email);
            if (calendars == null) return TypedResults.NotFound();

            var organized = calendars.Select(a => new CalendarListResponse(a.ID, a.Name, a.Description)).ToList();
            string asJson = JsonConvert.SerializeObject(organized);

            return TypedResults.Ok(asJson);
        }

        private static IResult GetEventsDuringTimeFrame(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? jsonBody
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (!Sessions.ValidateAuthentication(email, authentication)) return TypedResults.Unauthorized();

            if (jsonBody == null) return TypedResults.BadRequest("Missing body");
            CalendarEventRequest? request = JsonConvert.DeserializeObject<CalendarEventRequest>(jsonBody);
            if (request == null) return TypedResults.BadRequest("Malformed body");

            List<Event>? events = Database.GetEventsFromCalendarWithinTimeframe(request);
            if (events == null) return TypedResults.BadRequest("Malformed request");

            var organized = events.Select(a => new EventListResponse(a.ID, a.Name, a.Description, a.Location, a.Start, a.End)).ToList();
            string asJson = JsonConvert.SerializeObject(organized);

            return TypedResults.Ok(asJson);
        }

        private static IResult ChangePassword(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? newPassword
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (!Sessions.ValidateAuthentication(email, authentication)) return TypedResults.Unauthorized();

            if (newPassword == null) return TypedResults.BadRequest("Missing body");
            bool success = Accounts.ResetPassword(email, newPassword, newPassword);

            if (success) return TypedResults.Ok("Password changed");
            else return TypedResults.BadRequest();
        }

        private static IResult ShareCalendar(
            [FromHeader(Name = "Email")] string? email,
            [FromHeader(Name = "Authentication")] string? authentication,
            [FromBody] string? jsonBody
        )
        {
            if (email == null || authentication == null) return TypedResults.BadRequest();
            if (!Sessions.ValidateAuthentication(email, authentication)) return TypedResults.Unauthorized();
            if (jsonBody == null) return TypedResults.BadRequest("Missing body");

            var request = JsonConvert.DeserializeObject<CalendarShareRequest>(jsonBody);
            if (request == null) return TypedResults.BadRequest("Malformed body");

            bool success = CalendarManager.ShareCalendar(email, request);
            if (success) return TypedResults.Ok();
            else return TypedResults.Unauthorized();
        }

        private record LoginResponse(string authentication, string email);
        private record CalendarListResponse(int id, string? name, string? description);
        private record EventListResponse(int id, string? name, string? description, string? location, DateTime start, DateTime end);

    }

}
