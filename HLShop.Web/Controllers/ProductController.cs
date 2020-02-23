using AutoMapper;
using HLShop.Common;
using HLShop.Service;
using HLShop.Web.Infrastructure.Core;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HLShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }

        // GET: Product
        public ActionResult Detail(int productId)
        {
            var productModel = _productService.GetById(productId);

            IMapper mapper = AutoMapperConfiguration.Configure();
            var productViewModel = mapper.Map<ProductViewModel>(productModel);

            //related product
            var listRelatedProduct = _productService.GetListRelatedProduct(productId, 6);
            ViewBag.ListRelatedProductViewModel = mapper.Map<IEnumerable<ProductViewModel>>(listRelatedProduct);

            //more image
            var moreImages = productViewModel.MoreImages;
            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(moreImages);
            ViewBag.MoreImages = listImages; 

            return View(productViewModel);
        }

        public ActionResult Category(int id, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var listProductModel = _productService.GetListProductByCategoryIdPaging(id, page, sort, pageSize, out totalRow);

            IMapper mapper = AutoMapperConfiguration.Configure();
            var listProductViewModel = mapper.Map<IEnumerable<ProductViewModel>>(listProductModel);

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = mapper.Map<ProductCategoryViewModel>(category);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = listProductViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productService.GetListProductByName(keyword);

            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var listProductModel = _productService.Search(keyword, page, sort, pageSize, out totalRow);

            IMapper mapper = AutoMapperConfiguration.Configure();
            var listProductViewModel = mapper.Map<IEnumerable<ProductViewModel>>(listProductModel);

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Keyword = keyword;

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = listProductViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
    }
}