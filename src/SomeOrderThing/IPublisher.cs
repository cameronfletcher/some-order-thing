namespace SomeOrderThing
{
    public interface IPublisher
    {
        void Publish<T>(T message);
    }
}
