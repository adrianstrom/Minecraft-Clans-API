using ClansGrpcService.Services;
using ClansGrpcService.Validators;
using Database.Models;
using Database.Options;
using Database.Repositories;
using Database.Repositories.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ClansGrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            // Configure options
            var configurationRoot = builder.Configuration;
            builder.Services.Configure<ClansDatabaseOptions>(
                configurationRoot.GetSection(nameof(ClansDatabaseOptions)));

            // Add repositories to the container
            builder.Services.AddTransient<IClanRepository, ClanRepository>();
            builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();

            // Add validators to the container
            builder.Services.AddScoped<IValidator<Clan>, ClanValidator>();
            builder.Services.AddScoped<IValidator<Player>, PlayerValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<ClanService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}