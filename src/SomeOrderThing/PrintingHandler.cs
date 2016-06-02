namespace SomeOrderThing
{
    using System;

    public class PrintingHandler : IHandleOrder
    {
        public void Handle(TableOrder order)
        {
            ////Console.WriteLine(order.Serialize().ToString());
        }
    }
}
