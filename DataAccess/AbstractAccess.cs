public abstract class AbstractAccess<T>
{
    public abstract List<T> LoadAll();
    public abstract void WriteAll(List<T> items);
}