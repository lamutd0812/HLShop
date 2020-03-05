using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HLShop.Data.Infrastructure;
using HLShop.Model.Models;

namespace HLShop.Data.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetOrderDetailByOrderId(int orderId);

    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
            var query = from order in DbContext.Orders 
                join orderDetail in DbContext.OrderDetails
                on order.ID  equals orderDetail.OrderID
                where order.ID == orderId
                select orderDetail;
            return query.OrderByDescending(x => x.ProductID);
        }
    }
}