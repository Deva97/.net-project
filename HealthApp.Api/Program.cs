using HealthApp.Api.Middleware;
using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using HealthApp.Infrastructure;
using HealthApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HealthApp.Application.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

//before build
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();



builder.Services.Configure<DBConfigurationClass>(
    builder.Configuration.GetSection(DBConfigurationClass.SectionName));

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);

builder.Services.Configure<JwtOptions>(jwtSection);

var jwtOptions = jwtSection.Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };

    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});








builder.Host.ConfigureHostOptions(option =>
{
    option.ShutdownTimeout = TimeSpan.FromSeconds(5);
});



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


var lifeTime = app.Lifetime;

lifeTime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("App started");
});

lifeTime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("App stopping");
});

lifeTime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("App stopped");
});


app.UseMiddleware<HttpHeaderMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseRouting();

//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");


app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.Map("/health", healthContext =>
{
    healthContext.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"status\":\"Healthy\"}");
    });
});

app.Map("/Food", admin =>
{
    admin.Use(async (context, next) =>
    {
        Console.WriteLine("admin before next");

        await next();

        Console.WriteLine("admin after next");
    });

    admin.Use(async (context, next) =>
    {
        Console.WriteLine("admin 2 before next");

        await next();

        Console.WriteLine("admin 2 after next");
    });

    admin.Run(async context =>
    {
        await context.Response.WriteAsync("Admin area");
    });
});


var food = app.MapGroup("/Food").AddEndpointFilter(async (context,next) =>
{
    Console.WriteLine("Food endpoint called");
    return await next(context);
});

food.MapGet("/{id:int}", Results<Ok<Food>, NotFound> (int id) =>
{
    if (id > 0)
        return TypedResults.Ok(new Food()
        {
            Id = new Guid(),
            Name = "temp"

        });
    else return TypedResults.NotFound();
});


food.MapPost("/{id:int}", Results<Ok<Food>, InternalServerError> (int id) =>
{
    if (id > 0)
        return TypedResults.Ok(new Food()
        {
            Id = new Guid(),
            Name = "temp"

        });
    else return TypedResults.InternalServerError();
});




// this produce tells me hey I this api can return 200OK with Food

// we do this beofre the app is running and listening to the ports and reuqest

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
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
