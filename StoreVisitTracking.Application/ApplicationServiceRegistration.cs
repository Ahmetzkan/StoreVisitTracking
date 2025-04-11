using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StoreVisitTracking.Application.DTOs.Auth;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Application.Validators.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace StoreVisitTracking.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVisitService, VisitService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }

        //public static IServiceCollection AddSubClassesOfType(
        //    this IServiceCollection services,
        //    Assembly assembly,
        //    Type type,
        //    Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        //{
        //    var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        //    foreach (var item in types)
        //    {
        //        if (addWithLifeCycle == null)
        //        {
        //            services.AddScoped(item);
        //        }
        //        else
        //        {
        //            addWithLifeCycle(services, item);
        //        }
        //    }
        //    return services;
        //}
    }

}
