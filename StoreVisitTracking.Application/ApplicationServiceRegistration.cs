using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StoreVisitTracking.Application.DTOs.Auth;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Application.Settings;
using StoreVisitTracking.Application.Validators.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVisitService, VisitService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSettings>(settings =>
            {
                configuration.GetSection("Redis").Bind(settings);
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(configuration.GetSection("Redis:Configuration").Value));

            return services;
        }
    }
}