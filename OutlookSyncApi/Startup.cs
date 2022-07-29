using System.Data;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.OpenApi.Models;
using OutlookSyncApi.Migrations;
using OutlookSyncApi.Repositories;
using OutlookSyncApi.Services;

namespace OutlookSyncApi
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
            services.AddSingleton<DapperContext>();
            services.AddSingleton<Database>();
            services.AddLogging(c => c.AddFluentMigratorConsole())
                    .AddFluentMigratorCore()
                    .ConfigureRunner(c => c.AddSqlServer()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("SqlConnection"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OutlookSyncApi", Version = "v1" });
            });

            services.AddTransient<IOutlookCalendarService, OutlookCalendarService>();
            services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();
            services.AddTransient<IGraphConfigurationRepository, GraphConfigurationRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OutlookSyncApi v1"));
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

