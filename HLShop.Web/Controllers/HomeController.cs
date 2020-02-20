using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using HLShop.Service;
using HLShop.Web.Mappings;
using HLShop.Web.Models;

namespace HLShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProductCategoryService _productCategoryService;
        private ICommonService _commonService;
        public HomeController(IProductCategoryService productCategoryService, ICommonService commonService)
        {
            this._productCategoryService = productCategoryService;
            this._commonService = commonService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            IMapper _mapper = AutoMapperConfiguration.Configure();
            var footerViewModel = _mapper.Map<FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            //map sang ProductCategoryVm (responseData)
            IMapper _mapper = AutoMapperConfiguration.Configure();
            var listProductCategoryViewModel = _mapper.Map<List<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}