namespace Core
{
    public interface ILoader<T>
    {
        T Load(string fileName);
        bool Save(string fileName,T objectToSave);
    }
}