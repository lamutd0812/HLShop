using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using AutoMapper;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.Infrastructure.Core;
using HLShop.Web.Infrastructure.Extensions;
using HLShop.Web.Mappings;
using HLShop.Web.Models;

namespace HLShop.Web.Api
{
    [System.Web.Http.RoutePrefix("api/product")]
    public class ProductController : ApiControllerBase
    {
        #region Initialize

        private IProductService _productService; 

        public ProductController(IErrorService errorService, IProductService productService)
            : base(errorService)
        {
            this._productService = productService;
        }

        #endregion Initialize

        [System.Web.Http.Route("getall")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<List<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [System.Web.Http.Route("getallparents")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<List<ProductViewModel>>(model);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [System.Web.Http.Route("create")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProduct = new Product();
                    newProduct.UpdateProduct(productViewModel);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;

                    _productService.Add(newProduct);
                    _productService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [System.Web.Http.Route("getbyid/{id:int}")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<ProductViewModel>(model);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [System.Web.Http.Route("update")]
        [System.Web.Http.HttpPut]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct = _productService.GetById(productViewModel.ID);
                    dbProduct.UpdateProduct(productViewModel);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.UpdatedBy = User.Identity.Name;

                    _productService.Update(dbProduct);
                    _productService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [System.Web.Http.Route("delete")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldProduct = _productService.Delete(id);
                    _productService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductViewModel>(oldProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [System.Web.Http.Route("deletemulti")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedIds)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listProductId = new JavaScriptSerializer().Deserialize<List<int>>(checkedIds);
                    foreach (var item in listProductId)
                    {
                        var oldProductCategory = _productService.Delete(item);
                    }
                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductId.Count);
                }

                return response;
            });
        }
    }
}
