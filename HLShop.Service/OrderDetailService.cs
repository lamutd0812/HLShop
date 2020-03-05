using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLShop.Data.Infrastructure;
using HLShop.Data.Repositories;
using HLShop.Model.Models;

namespace HLShop.Service
{
    public interface IOrderDetailService
    {
        IEnumerable<OrderDetail> GetOrderDetailByOrderId(int orderId);
    }

    public class OrderDetailService: IOrderDetailService
    {
        private IOrderDetailRepository _orderDetailRepository;
        private IOrderRepository _orderRepository;
        private IUnitOfWork _unitOfWork;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._orderRepository = orderRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
            var model = _orderDetailRepository.GetOrderDetailByOrderId(orderId);
            return model;
        }
    }
}
