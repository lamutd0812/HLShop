using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using HLShop.Service;
using HLShop.Web.Infrastructure.Core;
using HLShop.Web.Mappings;
using HLShop.Web.Models;

namespace HLShop.Web.Api
{
    [System.Web.Http.RoutePrefix("api/orderdetail")]
    public class OrderDetailController : ApiControllerBase
    {
        private IOrderDetailService _orderDetailService;
        private IMapper _mapper;

        public OrderDetailController(IErrorService errorService, IOrderDetailService orderDetailService) : base(errorService)
        {
            this._orderDetailService = orderDetailService;
            this._mapper = AutoMapperConfiguration.Configure();
        }

        [System.Web.Http.Route("getbyorderid/{id}")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetByOrderId(HttpRequestMessage request,int id, int page, int pageSize = 2)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderDetailService.GetOrderDetailByOrderId(id);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ProductID).Skip(page * pageSize).Take(pageSize);

                //map sang PostCategoryVm (responseData)
                var responseData = _mapper.Map<List<OrderDetailViewModel>>(query);

                var paginationSet = new PaginationSet<OrderDetailViewModel>()
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
    }
}