using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;
using System.Text.Json.Serialization;
using BikeRentalAPI.Repositories;

namespace BikeRentalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database context
            builder.Services.AddDbContext<APIDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection")));

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBikeRepository, BikeRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRentalStatusRepository, RentalStatusRepository>();

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