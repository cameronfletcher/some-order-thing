namespace SomeOrderThing
{
    using System.Linq;

    public class AssistantManager : IHandle<Messages.OrderCooked>
    {
        private readonly IPublisher publisher;

        public AssistantManager(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public void Handle(Messages.OrderCooked order)
        {
            var tableOrder = order.Order.Copy();

            tableOrder.Tax = 12.4m;
            tableOrder.Total = tableOrder.LineItems.Sum(lineItem => lineItem.Quantity * lineItem.Price) + tableOrder.Tax;

            this.publisher.Publish(new Messages.OrderPriced { Order = tableOrder });
        }
    }
}
