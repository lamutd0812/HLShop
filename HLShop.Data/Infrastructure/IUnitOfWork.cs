namespace HLShop.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}