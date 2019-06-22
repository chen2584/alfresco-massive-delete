using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using MassiveDelele.Models;
using Serilog.Core;

namespace MassiveDelete.Services
{
    public class MainService
    {
        readonly AlfrescoService alfrescoService;
        readonly AppSetting setting;
        readonly Logger logger;
        static BlockingCollection<string> uuids = new BlockingCollection<string>();
        public MainService(AlfrescoService alfrescoService, AppSetting setting, Logger logger)
        {
            this.alfrescoService = alfrescoService;
            this.setting = setting;
            this.logger = logger;
        }

        private async Task CreateWorker(int maxWorker)
        {
            var tasks = Enumerable.Range(0, setting.WorkerNumber).Select(workerNo => Task.Run(async () =>
            {
                logger.Information("Worker: {0} has been created.", workerNo);
                while (!uuids.IsCompleted)
                {
                    try
                    {
                        var uuid = uuids.Take();
                        var success = await alfrescoService.DeleteNodeAsync(uuid);
                        logger.Information("Worker {0}: Node Id: {1}, Delete Status: {2}.", workerNo, uuid, (success ? "Success" : "Fail"));
                    }
                    catch (InvalidOperationException)
                    { }
                }
            }));

            await Task.WhenAll(tasks);
        }

        private async Task<int> RunTaskAsync(string searchQuery)
        {
            var searchOutput = await alfrescoService.SearchNodeAsync(searchQuery);
            if (searchOutput == null) return -1;

            if (searchOutput.List.Pagination.TotalItems == 0) uuids.CompleteAdding();

            logger.Information("Found {0}/{1} nodes (Total Node: {2}) from Alfresco.", searchOutput.List.Entries.Count(), setting.MaxSearchItem, searchOutput.List.Pagination.TotalItems);
            foreach (var uuid in searchOutput.List.Entries.Select(x => x.Entry.Id))
            {
                uuids.Add(uuid);
            }

            return searchOutput.List.Pagination.TotalItems;
        }

        public async Task StartAsync()
        {
            var worker = CreateWorker(setting.WorkerNumber);

            var registry = new Registry();
            var scheduleName = "RemoveFilesScheduler";
            registry.Schedule(() =>
            {
                try
                {
                    var found = RunTaskAsync(setting.SearchQuery).Result; // FluentScheduler not support async-await
                    if (found == 0) JobManager.RemoveJob(scheduleName);
                }
                catch (Exception e)
                {
                    logger.Error(e.ToString());
                }
            }).WithName(scheduleName).NonReentrant().ToRunOnceAt(DateTime.Now.AddSeconds(1)).AndEvery(setting.SearchDelay).Seconds();
            JobManager.Initialize(registry);

            if (!worker.IsCompleted) await worker;
        }
    }
}