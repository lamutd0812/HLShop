using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.Infrastructure.Core;
using HLShop.Web.Mappings;
using HLShop.Web.Models;

namespace HLShop.Web.Api
{ 
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listProductCategories = _productCategoryService.GetAll();

                //map sang PostCategoryVm
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var listProductCategoryVm = _mapper.Map<List<ProductCategoryViewModel>>(listProductCategories);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listProductCategoryVm);

                return response;
            });
        }
    }
}