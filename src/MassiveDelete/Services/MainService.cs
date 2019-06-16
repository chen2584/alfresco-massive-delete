using System.Threading.Tasks;
using MassiveDelele.Models;
using Serilog.Core;

namespace MassiveDelete.Services
{
    public class MainService
    {
        readonly AppSetting setting;
        readonly Logger logger;
        public MainService(AppSetting setting, Logger logger)
        {
            this.setting = setting;
            this.logger = logger;
        }
        
        public async Task StartAsync()
        {
            logger.Information("Main Service Started!");
            await Task.CompletedTask;
        }
    }
}