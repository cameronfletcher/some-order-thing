namespace SomeOrderThing
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;

    public class TopicBasedPubSub : IPublisher
    {
        private readonly ConcurrentDictionary<string, List<object>> handlers
            = new ConcurrentDictionary<string, List<object>>();

        public void Publish<T>(T message)
        {
            var topic = message.GetType().Name;
            List<object> handlers;

            if (this.handlers.TryGetValue(topic, out handlers))
            {
                foreach (var handler in handlers)
                {
                    var typedHandler = handler as IHandle<T>;
                    if (typedHandler != null)
                    {
                        typedHandler.Handle(message);
                    }
                }
            }
        }

        public void SubscribeByType<T>(IHandle<T> handler)
        {
            this.Subscribe(typeof(T).Name, handler);
        }

        public void Subscribe<T>(string topic, IHandle<T> handler)
        {
            int retries = 0;
            bool success;
            do
            {
                System.Console.WriteLine("Subscribe invoked for {0}, attempt {1}", topic, ++retries);

                var handlersOfT = this.handlers.GetOrAdd(topic, t => new List<object>());

                var list = new List<object>(handlersOfT);
                list.Add(handler);

                System.Threading.Thread.Sleep(2000);

                success = this.handlers.TryUpdate(topic, list, handlersOfT);
            }
            while (!success);
        }

        public void UnsubscribeByType<T>(IHandle<T> handler)
        {
            this.Unsubscribe(typeof(T).Name, handler);
        }

        public void Unsubscribe<T>(string topic, IHandle<T> handler)
        {
            bool success;
            do
            {
                List<object> handlersOfT;
                if (!this.handlers.TryGetValue(topic, out handlersOfT))
                {
                    return;
                }

                var list = new List<object>(handlersOfT);
                list.Remove(handler);

                success = this.handlers.TryUpdate(topic, list, handlersOfT);
            }
            while (!success);
        }
    }
}
