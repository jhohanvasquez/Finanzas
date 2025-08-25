using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finanzas.Api.Service;
using Finanzas.Infraestructure.Interfaces;
using Finanzas.Infraestructure.Repositories.Configuration;
using Finanzas.ServiceCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Repositories;
using StackExchange.Redis;
using System.IO; // <-- Importante para la ruta del XML

namespace Finanzas.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Swagger con comentarios XML
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Finanzas API",
                    Version = "v1",
                    Description = "Documentación de la API de Finanzas"
                });

                // Incluir comentarios XML
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // Registro de la conexión a la base de datos SQL
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            // Registro de la conexión a Redis 
            var redisConnectionString = Configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

            services.AddScoped<UsuarioRepository>();
            services.AddScoped<DeudaRepository>();
            services.AddScoped<PagoRepository>();
            services.AddScoped<CacheService>();

            services.AddTransient<IDeudaRepository, DeudaRepository>();
            services.AddTransient<IPagoRepository, PagoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAngular");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Redirigir la raíz a /swagger/index.html
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger/index.html");
                    return;
                }
                await next();
            });

            // Swagger Middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finanzas API v1");
                c.RoutePrefix = "swagger"; // Swagger UI en /swagger
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
