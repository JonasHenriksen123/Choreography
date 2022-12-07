using Accounts.Model;
using Accounts.ServiceAPI;

namespace Accounts.Services
{
    public class QueuedHostedService : BackgroundService
    {

        public QueuedHostedService(IBackgroundTaskQueue taskQueue) 
        {
            this.TaskQueue = taskQueue;
        }

        public IBackgroundTaskQueue TaskQueue { get; }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.backgroundProcessing(stoppingToken);
        }

        private async Task backgroundProcessing(CancellationToken cancellationToken)
        {
            //see if execution has been cancelled
            while(!cancellationToken.IsCancellationRequested) 
            {
                //get next task
                var task = await TaskQueue.DequeueAsync(cancellationToken);

                try
                {
                    //run task
                    await task(cancellationToken);
                }
                catch(Exception) 
                {
                    //this is bad, should be logged to catch processing errors
                }
            }
        }
    }
}
