using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Filters;
using FicWriter.API.Infrastructure.Mediator;
using FicWriter.API.Infrastructure.Security.Authorization;
using FicWriter.API.Shared.Mapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FicWriter API",
        Description = "API for the FicWriter application",
    });

    c.OperationFilter<WorkIdFilter>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConfiguredMediatR();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMappers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userId, out var id))
                {
                    context.Fail("Invalid token");
                    return;
                }

                var dbContext = context.HttpContext.RequestServices
                    .GetRequiredService<FicWriterDbContext>();

                var userExists = await dbContext.Users
                    .AnyAsync(u => u.UserIdentifier == id);

                if (!userExists)
                {
                    context.Fail("Invalid token");
                    return;
                }

                context.Success();
            },
            
            OnAuthenticationFailed = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                string message;

                if (context.Exception is SecurityTokenExpiredException)
                {
                    message = "Token expired";
                }
                else
                {
                    message = "Invalid token";
                }

                var result = new
                {
                    error = message
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SameUser", policy =>
    {
        policy.Requirements.Add(new SameUserRequirement());
    });

builder.Services.AddSingleton<IAuthorizationHandler, SameUserHandler>();
builder.Services.AddScoped<IAuthorizationHandler, WorkAuthHandler>();

builder.Services.AddGroupedEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroupedEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FicWriter API V1");
    });
}

app.UseBindingExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program
{
    protected Program() { }
}