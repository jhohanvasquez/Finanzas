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
using Repositories;
using StackExchange.Redis;

namespace Finanzas.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Registro de la conexión a la base de datos SQL
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            // Registro de la conexión a Redis 
            var options = ConfigurationOptions.Parse(Configuration.GetConnectionString("RedisConnection"));
            options.AbortOnConnectFail = false;
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));



            services.AddScoped<UsuarioRepository>();
            services.AddScoped<DeudaRepository>();
            services.AddScoped<PagoRepository>();
            services.AddScoped<CacheService>();

            services.AddTransient<IDeudaRepository, DeudaRepository>();
            services.AddTransient<IPagoRepository, PagoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("PermitirTodo");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
