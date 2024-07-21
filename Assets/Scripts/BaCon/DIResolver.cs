namespace BaCon
{
    public abstract class DIResolver<T> : DIEntry
    {
        public abstract T Resolve();
    }
}