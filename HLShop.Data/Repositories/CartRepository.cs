using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLShop.Data.Infrastructure;
using HLShop.Model.Models;

namespace HLShop.Data.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {

    }

    public class CartRepository: RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
