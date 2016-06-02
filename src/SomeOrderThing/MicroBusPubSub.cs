namespace SomeOrderThing
{
    internal class MicrobusPubSub : IPublisher
    {
        private readonly Microbus bus;

        public MicrobusPubSub(Microbus bus)
        {
            this.bus = bus;
        }

        public void Publish<T>(T message)
        {
            this.bus.Send(message);
        }
    }
}
