namespace SomeOrderThing
{
    using System;

    public class PrintingHandler : IHandle<Messages.OrderPaid>
    {
        public void Handle(Messages.OrderPaid order)
        {
            ////Console.WriteLine(order.Serialize().ToString());
        }
    }
}
