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
    public interface ICartService
    {
        bool Add (Cart cart);

        bool Update (Cart cart);

        bool Delete(Cart cart);

        bool DeleteAllItem(string userId);

        IEnumerable<Cart> GetCartItemByUser(string userId);

        void Save();
    }

    public class CartService : ICartService
    {
        private ICartRepository _cartRepository;
        private IUnitOfWork _unitOfWork;

        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this._cartRepository = cartRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool Add(Cart cart)
        {
            try
            {
                _cartRepository.Add(cart);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Update(Cart cart)
        {
            try
            {
                _cartRepository.Update(cart);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Delete(Cart cart)
        {
            try
            {
                _cartRepository.Delete(cart);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool DeleteAllItem(string userId)
        {
            try
            {
                _cartRepository.DeleteMulti(x => x.UserId.Equals(userId));
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<Cart> GetCartItemByUser(string userId)
        {
            return _cartRepository.GetMulti(x => x.UserId.Equals(userId)).OrderByDescending(x=>x.ProductId);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
