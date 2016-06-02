namespace SomeOrderThing
{
    using System.Threading;

    public class Cook : IHandle<Messages.OrderPlaced>
    {
        private readonly IPublisher publisher;
        private readonly string name;
        private readonly int sleepTime;

        public Cook(IPublisher publisher, string name, int sleepTime)
        {
            this.publisher = publisher;
            this.sleepTime = sleepTime;
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public void Handle(Messages.OrderPlaced order)
        {
            Thread.Sleep(this.sleepTime);

            var tableOrder = order.Order.Copy();

            tableOrder.Ingredients = "KFC chicken";
            tableOrder.CookName = this.name;

            this.publisher.Publish(new Messages.OrderCooked { Order = tableOrder });
        }
    }
}
