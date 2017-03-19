using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;

namespace Kontur.GameStats.Server
{
    public static class Program
    {
        public static float HasValue(this float value, float other)
        {
            return float.IsNaN(value) || float.IsInfinity(value) ? other : value;
        }

        public static bool IsValidURI(string uri)
        {
            return Regex.IsMatch(uri ?? "", @"^https?://[^/]", RegexOptions.IgnoreCase);
        }

        public static void Main(string[] args)
        {
            var cmdLineApp = new CommandLineApplication(false);

            var prefix = cmdLineApp.Option(
              "-p | --prefix <prefix>",
              "Set default prefix for the http/https adapter",
              CommandOptionType.SingleValue);

            cmdLineApp.HelpOption("-? | -h | --help");

            cmdLineApp.OnExecute(() =>
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>();

                var uri = prefix.Value();
                if (IsValidURI(uri))
                    host.UseUrls(uri);

                host
                    .Build()
                    .Run();

                return 0;
            });

            cmdLineApp.Execute(args);
        }
    }
}