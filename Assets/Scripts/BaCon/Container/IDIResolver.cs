namespace BaCon
{
    public interface IDIResolver
    {
        T Resolve<T>(string tag = null);
    }
}