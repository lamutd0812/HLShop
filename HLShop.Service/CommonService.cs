using HLShop.Model.Models;
using System;
using HLShop.Common;
using HLShop.Data.Infrastructure;
using HLShop.Data.Repositories;

namespace HLShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
    }

    public class CommonService : ICommonService
    {
        private IFooterRepository _footerRepository;
        private IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerRepository, IUnitOfWork unitOfWork)
        {
            this._footerRepository = footerRepository;
            this._unitOfWork = unitOfWork;
        }

        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }
    }
}