namespace SomeOrderThing
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class ThreadedHandler<T> : IHandle<T>, IStartable, IDisposable
    {
        private readonly ConcurrentQueue<T> orders = new ConcurrentQueue<T>();
        private readonly IHandle<T> handler;
        private Thread thread;

        public ThreadedHandler(IHandle<T> handler)
        {
            this.handler = handler;
        }

        public void Handle(T order)
        {
            this.orders.Enqueue(order);
        }

        public void Start()
        {
            this.thread = new Thread(new ThreadStart(DoStuff));
            thread.Start();
        }

        private void DoStuff()
        {
            while (true)
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
            this.thread.Abort();
        }
    }
}
