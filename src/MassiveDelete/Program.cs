using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MassiveDelele.Models;
using MassiveDelete.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace MassiveDelete
{
    class Program
    {
        static AppSetting LoadSetting()
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(rootPath, "appsettings.json");
            var setting = JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText(filePath));
            return setting;
        }

        static Logger GetLogger()
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: Path.Combine(rootPath, "logs", "log-.log"),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1_000_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();
            return logger;
        }

        public static async Task Main(string[] args)
        {
            var logger = GetLogger();
            logger.Information("Starting Service...");

            var setting = LoadSetting();
            var serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<Logger>(logger)
                .AddSingleton<AppSetting>(setting)
                .AddSingleton<AlfrescoService>()
                .AddSingleton<MainService>()
                .BuildServiceProvider();

            var mainService = serviceProvider.GetService<MainService>();
            await mainService.StartAsync();
            
            logger.Information("Stopped Service...Press any key to exit.");
            Console.ReadLine();
        }
    }
}
