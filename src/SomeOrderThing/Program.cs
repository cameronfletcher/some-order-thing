namespace SomeOrderThing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var bus = new Microbus();
            var publisher = new MicrobusPublisher(bus);

            var printer = new PrintingHandler();
            var cashier = new TaskThreadedHandler<Messages.OrderPriced>(new Cashier(publisher), "cashier");
            var assMan = new TaskThreadedHandler<Messages.OrderCooked>(new AssistantManager(publisher), "assMan");

            var random = new Random();
            var cooks = new[]
            {
                new TaskThreadedHandler<Messages.OrderPlaced>(new Cook(publisher, "Guybrush Threepwood", random.Next(500, 3000)), "Guybrush Threepwood"),
                new TaskThreadedHandler<Messages.OrderPlaced>(new Cook(publisher, "Elaine Marley", random.Next(500, 3000)), "Elaine Marley"),
                new TaskThreadedHandler<Messages.OrderPlaced>(new Cook(publisher, "Zombie Pirate LeChuck", random.Next(500, 3000)), "Zombie Pirate LeChuck")
            };

            var dispatcher = new TaskThreadedHandler<Messages.OrderPlaced>(new MoreFairDispatcher<Messages.OrderPlaced>(cooks), "More fair handler");
            var waiter = new Waiter(publisher);

            var list = new List<IStartable>();
            list.AddRange(cooks, cashier, assMan, dispatcher);

            bus.AutoRegister(dispatcher, assMan, cashier, printer);
            //publisher.Subscribe(cookTopic, dispatcher);
            //publisher.Subscribe(priceOrderTopic, assMan);
            //publisher.Subscribe(takePaymentTopic, cashier);
            //publisher.Subscribe(printOrderTopic, printer);

            var cts = new CancellationTokenSource();
            Task.Run(() => MonitorStuff(list.Cast<IMonitorable>(), cts.Token));

            list.ForEach(item => item.Start());

            for (var i = 0; i < 50; i++)
            {
                var order = new TableOrder(Guid.NewGuid());
                waiter.Handle(order);
            }

            Console.ReadLine();

            cts.Cancel();

            list.ForEach(item => item.Dispose());
        }

        private static void MonitorStuff(IEnumerable<IMonitorable> handlers, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var handler in handlers)
                {
                    Console.Write("{0}: {1}, ", handler.Name, handler.Count);
                }

                Console.WriteLine();

                Thread.Sleep(1000);
            }
        }
    }
}
