//namespace SomeOrderThing
//{
//    using System;
//    using System.Collections.Generic;

//    public class RoundRobinDispatcher : IHandleOrder
//    {
//        private readonly Queue<IHandleOrder> handlers;

//        public RoundRobinDispatcher(IEnumerable<IHandleOrder> handlers)
//        {
//            this.handlers = new Queue<IHandleOrder>(handlers);

//            if (this.handlers.Count <= 0)
//            {
//                throw new ArgumentException("No handlers!");
//            }
//        }

//        public void Handle(TableOrder order)
//        {
//            var handler = this.handlers.Dequeue();

//            try
//            {
//                handler.Handle(order);
//            }
//            finally
//            {
//                this.handlers.Enqueue(handler);
//            }
//        }
//    }
//}
