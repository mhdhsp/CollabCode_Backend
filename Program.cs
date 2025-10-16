
using CollabCode.Data;
using CollabCode.Models;
using CollabCode.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CollabCode.Common.DTO;
using CollabCode.Common.Exceptions;

namespace CollabCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // logger for pgm.cs before build
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger("startup");

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAuthService, AuthService>();
            //Jwt Configuration

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();
            // Hanndling Validation  model Errors
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    var response = new ApiResponse<string>(string.Join("; ", errors));
                    logger.LogError("Validation failed: {Errors}", string.Join("; ", errors));
                    return new BadRequestObjectResult(response);
                };
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            //logger after build 
            var logg = app.Services.GetRequiredService<ILogger<Program>>();


            //Middleware for Globel Exception handling
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var er = feature?.Error;
                    var statuscode = er switch
                    {
                        ArgumentException => StatusCodes.Status400BadRequest,
                        UserAlreadyExistsException => StatusCodes.Status409Conflict,
                        UserNotFoundException=>StatusCodes.Status404NotFound,
                        MismatchException=>StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    
                   var resp=new 
                   {
                        Succes = false,
                        Message = er?.Message ?? "Somthing went error",
                        Path = feature?.Path,
                        statuscode
                    };
                    logg.LogError(er, "Global exception occurred at path {Path} with status code {StatusCode}", feature?.Path, statuscode);
                    await context.Response.WriteAsJsonAsync(resp);
                });
            });

            

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
        }
    }
}
