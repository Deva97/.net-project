using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using HealthApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MediatR;
using System.Reflection;
using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using HealthApp.Application.Command;
using HealthApp.Infrastructure.Services;

namespace HealthApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configureOptions)
        {
            services.AddDbContext<HealthAppDbContext>(options =>
            {
                options.UseMySql(configureOptions.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 36)));
            });

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            service.AddAutoMapper(option =>
            {
                option.CreateMap<Food, AddFoodMacros.Request>();
            });

            service.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<HealthAppDbContext>());
             
            service.AddScoped<IFoodRepository>(provider => provider.GetRequiredService<FoodRepository>());

            service.AddScoped<TokenService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddTransient<ITokenService, TokenService>();

            return service;
        }
    }
}
