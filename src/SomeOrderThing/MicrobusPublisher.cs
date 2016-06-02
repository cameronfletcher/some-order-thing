namespace SomeOrderThing
{
    using Messages;

    internal class MicrobusPublisher : IPublisher
    {
        private readonly Microbus bus;

        public MicrobusPublisher(Microbus bus)
        {
            this.bus = bus;
        }

        public void Publish(MessageBase message)
        {
            this.bus.Send(message);
        }
    }
}
