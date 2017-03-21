using System;
using Kontur.GameStats.Server.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Kontur.GameStats.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(connection));

            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("endpoint", typeof(EndpointConstraint)));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Можно просто перенести строку Database.Migrate() в конструктор DatabaseContext
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
            }

            // Логи в консоль вывода Visual Studio, если отлаживаем в IIS Express
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            app.UseMvc();
        }
    }

    public class EndpointConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            var endpoint = values[routeKey].ToString();

            var pos = endpoint.LastIndexOf('-');
            if (pos == -1) return false;

            var port = endpoint.Substring(pos + 1);
            var host = endpoint.Substring(0, pos);

            int q;
            return int.TryParse(port, out q)
                && q >= 0
                && q <= 65535
                && Uri.CheckHostName(host) != UriHostNameType.Unknown;
        }
    }
}