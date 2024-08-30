
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db;
using NotePadAPI.Db.IDb;
using Serilog;

namespace NotePadAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add Db contexts
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

            // Add Serilog configuration
            builder.Host.UseSerilog((context, config) => 
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // Controller options
            builder.Services.AddControllers(
                op => op.Filters.Add(new ProducesAttribute("application/json"))
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Add Serilog configuration
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
