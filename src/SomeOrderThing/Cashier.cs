namespace SomeOrderThing
{
    public class Cashier : IHandle<Messages.OrderPriced>
    {
        private readonly IPublisher publisher;

        public Cashier(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public void Handle(Messages.OrderPriced order)
        {
            var tableOrder = order.Order.Copy();
            tableOrder.Paid = true;
            this.publisher.Publish(new Messages.OrderPaid { Order = tableOrder });
        }
    }
}
