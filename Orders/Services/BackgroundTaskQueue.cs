using Orders.ServiceAPI;
using System.Threading.Channels;

namespace Orders.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, ValueTask>> queue;

        public BackgroundTaskQueue(int capacity) 
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            var task = await queue.Reader.ReadAsync(cancellationToken);

            return task;
        }

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> task)
        {
            if (task == null)
            {
                throw new ArgumentException(nameof(task));
            }

            await queue.Writer.WriteAsync(task);
        }
    }
}
