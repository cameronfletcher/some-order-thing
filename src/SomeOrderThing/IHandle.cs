namespace SomeOrderThing
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}
