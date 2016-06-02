namespace SomeOrderThing
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskThreadedHandler<T> : IHandle<T>, IStartable, IDisposable, IMonitorable
    {
        private readonly ConcurrentQueue<T> orders = new ConcurrentQueue<T>();
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private readonly IHandle<T> handler;

        public TaskThreadedHandler(IHandle<T> handler, string name)
        {
            this.handler = handler;
            this.Name = name;
        }

        public string Name { get; private set; }

        public int Count
        {
            get { return this.orders.Count; }
        }

        public void Handle(T order)
        {
            this.orders.Enqueue(order);
        }

        public void Start()
        {
            Task.Factory.StartNew(() => this.DoStuff(this.tokenSource.Token), TaskCreationOptions.LongRunning);
        }

        private void DoStuff(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                T order;
                if (this.orders.TryDequeue(out order))
                {
                    try
                    {
                        this.handler.Handle(order);
                    }
                    catch (Exception)
                    {
                    }
                }

                Thread.Sleep(1);
            }
        }

        public void Dispose()
        {
            this.tokenSource.Cancel();
        }
    }
}
