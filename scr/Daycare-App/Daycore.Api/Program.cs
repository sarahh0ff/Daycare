using AutoMapper;
using Daycare.Application.Interfaces;
using Daycare.Application.Mapping;
using Daycare.Application.Services;
using Daycare.Infrastructure.Context;
using Daycare.Infrastructure.Interfaces;
using Daycare.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;





internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<DaycareDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(typeof(DaycareProfile));

        // Repos
        builder.Services.AddScoped<IChildRepository, ChildRepository>();
        builder.Services.AddScoped<IGuardianRepository, GuardianRepository>();
        builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

        // Unit of Work
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add services to the container
        builder.Services.AddScoped<IChildService, ChildService>();
        builder.Services.AddScoped<IGuardianService, GuardianService>();
        builder.Services.AddScoped<IActivityService, ActivityService>();
        builder.Services.AddScoped<IAttendanceService, AttendanceService>();

        // Controllers
        builder.Services.AddControllers();


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}