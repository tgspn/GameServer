using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameServer.BD;
using GameServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GameServer
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
            Console.WriteLine("Setup Database");
            ApplicationDbContext.SetConfiguration(GetDatabaseType(), Configuration.GetConnectionString(nameof(ApplicationDbContext)));
            Console.WriteLine("Setup MemoryDB");
            GameMemoryDB.Instance.SetPersistenceInterval(GetPersistenceTime());
            services.AddControllers();

            services.AddSwaggerGen(options=>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                option.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            LeaderboardService.Instance.Start();
        }
        private DatabaseType GetDatabaseType()
        {
            return Enum.TryParse<DatabaseType>(Configuration["databaseType"], out var type) ? type : DatabaseType.Default;
        }
        private TimeSpan GetPersistenceTime()
        {
            return string.IsNullOrEmpty(Configuration["persistenceTime"]) ? TimeSpan.FromMinutes(10) : TimeSpan.Parse(Configuration["persistenceTime"]);
        }
    }
}
