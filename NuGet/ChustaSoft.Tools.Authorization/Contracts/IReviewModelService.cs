namespace ChustaSoft.Tools.Authorization
{
    public interface IReviewModelService<T>
    {
        void Review(T model);
    }
}
