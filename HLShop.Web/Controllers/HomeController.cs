using AutoMapper;
using HLShop.Common;
using HLShop.Service;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

namespace HLShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProductCategoryService _productCategoryService;
        private IProductService _productService;
        private ICommonService _commonService;

        public HomeController(IProductCategoryService productCategoryService, IProductService productService, ICommonService commonService)
        {
            this._productCategoryService = productCategoryService;
            this._productService = productService;
            this._commonService = commonService;
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            IMapper _mapper = AutoMapperConfiguration.Configure();

            var slideModels = _commonService.GetSlides();
            var listSlideViewModel = _mapper.Map<IEnumerable<SlideViewModel>>(slideModels);

            var lastestProductModels = _productService.GetLastest(3);
            var listLastestProductViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(lastestProductModels);

            var topSaleProductModels = _productService.GetHotProduct(3);
            var listTopSaleProductViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(topSaleProductModels);

            var homeViewModel = new HomeViewModel();
            homeViewModel.Slides = listSlideViewModel;
            homeViewModel.LastestProducts = listLastestProductViewModel;
            homeViewModel.TopSaleProducts = listTopSaleProductViewModel;

            try
            {
                //add SEO attributes
                homeViewModel.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View(homeViewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            IMapper _mapper = AutoMapperConfiguration.Configure();
            var footerViewModel = _mapper.Map<FooterViewModel>(footerModel);

            //ViewBag.Time = DateTime.Now.ToString("T");

            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult ListCategory()
        {
            var model = _productCategoryService.GetAll();
            //map sang ProductCategoryVm (responseData)
            IMapper _mapper = AutoMapperConfiguration.Configure();
            var listProductCategoryViewModel = _mapper.Map<List<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}