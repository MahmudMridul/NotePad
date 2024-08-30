
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db;
using NotePadAPI.Db.IDb;
using NotePadAPI.Repository;
using NotePadAPI.Repository.IRepository;
using Serilog;

namespace NotePadAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Db contexts
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

            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Serilog configuration
            builder.Host.UseSerilog((context, config) => 
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // CORS
            builder.Services.AddCors(op => 
                op.AddPolicy(
                    "AllowAll",
                    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                )
            );

            // Controller options
            builder.Services.AddControllers(
                op => op.Filters.Add(new ProducesAttribute("application/json"))
            );

            // Swagger/OpenAPI configuration. (https://aka.ms/aspnetcore/swashbuckle)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
