namespace SomeOrderThing
{
    public class Waiter
    {
        private readonly IPublisher publisher;

        public Waiter(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public void Handle(TableOrder order)
        {
            var tableOrder = order.Copy();

            tableOrder.TableNumber = 17;
            tableOrder.LineItems.Add(new TableOrder.LineItem { Quantity = 1, Item = "KFC please", Price = 4m });

            this.publisher.Publish(new Messages.OrderPlaced { Order = tableOrder });
        }
    }
}
