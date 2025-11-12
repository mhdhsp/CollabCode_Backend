using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Infrastructure.Persistense;
using CollabCode.CollabCode.WebApi.MiddleWare;
using CollabCode.CollabCode.WebApi.Common;
using CollabCode.CollabCode.Application.Mappings;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Infrastructure.Respositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Application.Services;
using Quartz;
using System.Threading.Tasks;
using Quartz.Impl;
using CollabCode.CollabCode.WebApi.Jobs;
using CollabCode.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using CollabCode.CollabCode.WebApi.Hubs;
//using CollabCode.CollabCode.Application.Services;
//using CollabCode.CollabCode.Application.Services;

namespace CollabCode
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


           


            
         
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            builder.Services.AddSignalR();


            // logger for pgm.cs before build
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger("startup");

            //configuring swagger authorization
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "CollabCode API", Version = "v1" });


                var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Description = "Enter 'Bearer' followed by your JWT token.",
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Id = "Bearer",
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                      {
                          new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                          {
                              Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                  Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                 }
                           },
                       Array.Empty<string>()
                         }
                 });
            });
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = Context =>
                    {
                        var token = Context.Request.Query["access_token"];
                        var path = Context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs/notification"))
                        {
                            Context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
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


            //Configuring CORS
            var frontendUrl = builder.Configuration.GetValue<string>("Frontend:Url");
            var swaggerUrl = builder.Configuration.GetValue<string>("Swagger:Url");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .WithOrigins(frontendUrl) // allow only your frontend
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });



            //IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            //await scheduler.Start();
            //IJobDetail job = JobBuilder.Create<FileChecking>()
            //.WithIdentity("FileChecking", "group1")
            //.Build();

            //// Define the trigger (run every 10 seconds)
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("myTrigger", "group1")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(60)
            //        .RepeatForever())
            //    .Build();

            //// Schedule the job
            //await scheduler.ScheduleJob(job, trigger);

            //Console.ReadKey(); // keep app running




            var app = builder.Build();

            using(var scope= app.Services.CreateAsyncScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                }
                catch(Exception ex)
                {
                     logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                    throw;
                }
            }

            app.UseCors("FrontendPolicy");

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
                        AlreadyExistsException => StatusCodes.Status409Conflict,
                        NotFoundException=>StatusCodes.Status404NotFound,
                        MismatchException=>StatusCodes.Status400BadRequest,
                        UnauthorizedAccessException=>StatusCodes.Status401Unauthorized,
                        BadHttpRequestException=>StatusCodes.Status400BadRequest,
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

            app.UseAuthentication();
            app.UseMiddleware<UserContextMiddleWare>();
            
            app.UseAuthorization();
            app.UseMiddleware<AuthMiddleware>();
           

            

            app.MapControllers();
            app.MapHub<ProjectHub>("/hubs/project");
            app.MapHub<ChatHub>("/hubs/chat");
            app.MapHub<NotificationHub>("/hubs/notification");


            app.Run();
        }
    }
}
