﻿using Entities.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contracts;
using Services.Manager;

namespace bsStoreApp.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// ConnectionString tanımlaması için tanımlanmış extension
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) => services.AddDbContext<RepositoryContext>(   ///Oluşturulmuş repository bilgisi burada tanımlanır ve generic olarak repository class verilir.
                db => db.UseSqlServer(configuration.GetConnectionString("sqlConnection")  ///appsettings.json içerisinde verdiğimiz connectionstring tanımlamasını burada tanımlayarak kullanıma alıyoruz. ---IOC(Inversion of Control - Konrolü Tersine Çevirme)  dahilinde yapılmıştır.
                ));

        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureLoggerService(this IServiceCollection service) => service.AddSingleton<ILoggerService, LoggerManager>();
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();     //Custsom Validation Attribute yapısının IOC ye kaydının yapılması
            services.AddSingleton<LogFilterAttribute>();    //Log Filter Yapısının eklenmesi
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(
                opt => opt.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination")

                )
            );
        }
        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();      
        }

    }
}
