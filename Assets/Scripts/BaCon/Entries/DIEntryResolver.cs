namespace BaCon
{
    public abstract class DIEntryResolver<T> : DIEntry
    {
        public abstract T Resolve();
    }
}