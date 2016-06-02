namespace SomeOrderThing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class MoreFairDispatcher<T> : IHandle<T>
    {
        private readonly Queue<TaskThreadedHandler<T>> handlers;

        public MoreFairDispatcher(IEnumerable<TaskThreadedHandler<T>> handlers)
        {
            this.handlers = new Queue<TaskThreadedHandler<T>>(handlers);

            if (this.handlers.Count <= 0)
            {
                throw new ArgumentException("No handlers!");
            }
        }

        public void Handle(T order)
        {
            while (true)
            {
                for (var x = 0; x < this.handlers.Count; x++)
                {
                    var handler = this.handlers.Dequeue();
                    if (handler.Count < 5)
                    {
                        try
                        {
                            handler.Handle(order);
                        }
                        finally
                        {
                            this.handlers.Enqueue(handler);
                        }

                        return;
                    }

                    this.handlers.Enqueue(handler);
                }

                Thread.Sleep(1);
            }
        }
    }
}
