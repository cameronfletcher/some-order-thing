namespace SomeOrderThing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Messages;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var bus = new Microbus();
            var publisher = new MicrobusPubSub(bus);

            var printer = new PrintingHandler();
            var cashier = new TaskThreadedHandler<OrderPriced>(new Cashier(publisher), "cashier");
            var assMan = new TaskThreadedHandler<OrderCooked>(new AssistantManager(publisher), "assMan");

            var random = new Random();
            var cooks = new[]
            {
                new TaskThreadedHandler<OrderPlaced>(new Cook(publisher, "Guybrush Threepwood", random.Next(500, 3000)), "Guybrush Threepwood"),
                new TaskThreadedHandler<OrderPlaced>(new Cook(publisher, "Elaine Marley", random.Next(500, 3000)), "Elaine Marley"),
                new TaskThreadedHandler<OrderPlaced>(new Cook(publisher, "Zombie Pirate LeChuck", random.Next(500, 3000)), "Zombie Pirate LeChuck")
            };

            var dispatcher = new TaskThreadedHandler<OrderPlaced>(
                new MoreFairDispatcher<OrderPlaced>(cooks), "More fair handler");

            var waiter = new Waiter(publisher);

            var list = new List<IMonitorable>();
            list.AddRange(cooks, cashier, assMan, dispatcher);

            bus.AutoRegister(dispatcher, assMan, cashier, printer);

            var cts = new CancellationTokenSource();
            Task.Run(() => MonitorStuff(list, cts.Token));

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
