namespace ChustaSoft.Tools.Authorization
{
    public interface IRevisable<T>
    {
        void Review(T model);
    }
}
