using Microsoft.AspNetCore.Hosting;
using Microsoft​.Extensions​.CommandLineUtils;
using System.Text.RegularExpressions;

namespace Kontur.GameStats.Server
{
    public class Program
    {
        public static bool IsValidURI(string uri)
        {
            string pattern = @"^https?://[^/]"; // TODO: Дописать регулярное выражение
            return Regex.IsMatch(uri ?? "", pattern, RegexOptions.IgnoreCase);
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

                string uri = prefix.Value();
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
