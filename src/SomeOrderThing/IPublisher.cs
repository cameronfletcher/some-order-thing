namespace SomeOrderThing
{
    public interface IPublisher
    {
        void Publish(Messages.MessageBase message);
    }
}
