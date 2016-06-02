﻿namespace SomeOrderThing
{
    using Messages;

    internal class MicrobusPublisher : IPublisher
    {
        private readonly Microbus bus;

        public MicrobusPublisher(Microbus bus)
        {
            this.bus = bus;
        }

        public void Publish<T>(T message)
        {
            this.bus.Send(message);
        }
    }
}