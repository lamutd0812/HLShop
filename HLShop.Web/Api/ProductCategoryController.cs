using AutoMapper;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.Infrastructure.Core;
using HLShop.Web.Infrastructure.Extensions;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HLShop.Web.Api
{
    [System.Web.Http.RoutePrefix("api/productcategory")]
    [Authorize]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize

        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion Initialize

        [System.Web.Http.Route("getall")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<List<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
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
                var model = _productCategoryService.GetAll();

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<List<ProductCategoryViewModel>>(model);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [System.Web.Http.Route("create")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel)
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
                    var newProductCategory = new ProductCategory();
                    newProductCategory.UpdateProductCategory(productCategoryViewModel);
                    newProductCategory.CreatedDate = DateTime.Now;
                    newProductCategory.CreatedBy = User.Identity.Name;

                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductCategoryViewModel>(newProductCategory);
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
                var model = _productCategoryService.GetById(id);

                //map sang PostCategoryVm (responseData)
                IMapper _mapper = AutoMapperConfiguration.Configure();
                var responseData = _mapper.Map<ProductCategoryViewModel>(model);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [System.Web.Http.Route("update")]
        [System.Web.Http.HttpPut]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel)
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
                    var dbProductCategory = _productCategoryService.GetById(productCategoryViewModel.ID);
                    dbProductCategory.UpdateProductCategory(productCategoryViewModel);
                    dbProductCategory.UpdatedDate = DateTime.Now;
                    dbProductCategory.UpdatedBy = User.Identity.Name;

                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductCategoryViewModel>(dbProductCategory);
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
                    var oldProductCategory = _productCategoryService.Delete(id);
                    _productCategoryService.Save();

                    IMapper _mapper = AutoMapperConfiguration.Configure();
                    var responseData = _mapper.Map<ProductCategoryViewModel>(oldProductCategory);
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
                    var listProductCategoryId = new JavaScriptSerializer().Deserialize<List<int>>(checkedIds);
                    foreach (var item in listProductCategoryId)
                    {
                        var oldProductCategory = _productCategoryService.Delete(item);
                    }
                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategoryId.Count);
                }

                return response;
            });
        }
    }
}