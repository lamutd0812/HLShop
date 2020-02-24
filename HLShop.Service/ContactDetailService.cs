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
    public interface IContactDetailService
    {
        ContactDetail GetDefaultContact();
    }

    public class ContactDetailService: IContactDetailService
    {
        private IContactDetailRepository _cdRepository;
        private IUnitOfWork _unitOfWork;

        public ContactDetailService(IContactDetailRepository cdRepository,
            IUnitOfWork unitOfWork)
        {
            this._cdRepository = cdRepository;
            this._unitOfWork = unitOfWork;
        }

        public ContactDetail GetDefaultContact()
        {
            return _cdRepository.GetSingleByCondition(x => x.Status == true);
        }
    }
}
