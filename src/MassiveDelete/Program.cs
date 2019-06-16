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
                .WriteTo.File(Path.Combine(rootPath, "log-.log"))
                .CreateLogger();
            return logger;
        }

        static async Task Main(string[] args)
        {
            var logger = GetLogger();
            logger.Information("Starting Service...");
            
            var setting = LoadSetting();
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Logger>(logger)
                .AddSingleton<AppSetting>(setting)
                .AddSingleton<MainService>()
                .BuildServiceProvider();

            var mainService = serviceProvider.GetService<MainService>();
            await mainService.StartAsync();
            logger.Information("Stopped Service...");
        }
    }
}
