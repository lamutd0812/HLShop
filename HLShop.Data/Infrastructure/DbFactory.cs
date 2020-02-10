namespace HLShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private HLShopDbContext dbContext;

        public HLShopDbContext Init()
        {
            return dbContext ?? (dbContext = new HLShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}