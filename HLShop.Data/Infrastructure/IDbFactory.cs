using System;

namespace HLShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HLShopDbContext Init();
    }
}