
using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db;
using NotePadAPI.Db.IDb;

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

            builder.Services.AddControllers();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
