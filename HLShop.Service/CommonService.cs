using HLShop.Model.Models;
using System;
using System.Collections.Generic;
using HLShop.Common;
using HLShop.Data.Infrastructure;
using HLShop.Data.Repositories;

namespace HLShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlides();
        SystemConfig GetSystemConfig(string code);
    }

    public class CommonService : ICommonService
    {
        private IFooterRepository _footerRepository;
        private ISlideRepository _slideRepository;
        private ISystemConfigRepository _systemConfigRepository;
        private IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerRepository, ISlideRepository slideRepository,
            ISystemConfigRepository systemConfigRepository,IUnitOfWork unitOfWork)
        {
            this._footerRepository = footerRepository;
            this._unitOfWork = unitOfWork;
            this._slideRepository = slideRepository;
            this._systemConfigRepository = systemConfigRepository;
        }

        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status==true);
        }

        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }
    }
}