
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotePadAPI.Db;
using NotePadAPI.Db.IDb;
using NotePadAPI.Repository;
using NotePadAPI.Repository.IRepository;
using Serilog;
using System.Text;

namespace NotePadAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Db Context Configurations
            bool useInMemoryDb = builder.Configuration.GetValue<bool>("UseInMemoryDb");
            if (useInMemoryDb)
            {
                builder.Services.AddDbContext<InMemoryContext>(
                    op => op.UseInMemoryDatabase("InMemoryDb")
                );
                builder.Services.AddScoped<IDbContext, InMemoryContext>();
            }
            else
            {
                builder.Services.AddDbContext<NotePadContext>(
                    op => op.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                );
                builder.Services.AddScoped<IDbContext, NotePadContext>();
            }
            #endregion

            #region Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            #endregion

            #region Serilog Configurations
            builder.Host.UseSerilog((context, config) => 
            {
                config.ReadFrom.Configuration(context.Configuration);
            });
            #endregion

            #region CORS
            builder.Services.AddCors(op =>
                op.AddPolicy(
                    "AllowAll",
                    policy => policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                )
            );
            #endregion

            #region Controller Configurations

            builder.Services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 5 * 1024 * 1024; // 5MB
            });

            builder.Services.AddControllers(
                op => op.Filters.Add(new ProducesAttribute("application/json"))
            );
            #endregion

            #region JWT Configurations
            builder.Services.AddAuthentication(ops =>
            {
                ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(ops =>
            {
                ops.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                };

                ops.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if(context.Request.Cookies.ContainsKey("AuthToken"))
                        {
                            context.Token = context.Request.Cookies["AuthToken"];
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    //OnTokenValidated = context =>
                    //{
                    //    Console.WriteLine($"OnTokenValidated: {context.SecurityToken}");
                    //    return Task.CompletedTask;
                    //}
                };
            });
            #endregion

            #region Swagger OpenAi Configurations
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotePad API", Version = "v1" });

                // JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            #endregion

            var app = builder.Build();

            // HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Serilog configuration
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            // CORS
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
