//namespace SomeOrderThing
//{
//    using System.Collections.Generic;

//    public class Multiplexer : IHandleOrder
//    {
//        private readonly IEnumerable<IHandleOrder> orderHandlers;

//        public Multiplexer(IEnumerable<IHandleOrder> orderHandlers)
//        {
//            this.orderHandlers = orderHandlers;
//        }
//        public void Handle(TableOrder order)
//        {
//            foreach (var orderHandler in this.orderHandlers)
//            {
//                orderHandler.Handle(order);
//            }
//        }
//    }
//}
