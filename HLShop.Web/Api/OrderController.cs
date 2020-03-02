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
    [System.Web.Http.RoutePrefix("api/order")]
    public class OrderController: ApiControllerBase
    {
        private IOrderService _orderService;
        private IMapper _mapper;

        public OrderController(IErrorService errorService, IOrderService orderService): base(errorService)
        {
            this._orderService = orderService;
            this._mapper = AutoMapperConfiguration.Configure();
        }

        [System.Web.Http.Route("getall")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 2)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                //map sang PostCategoryVm (responseData)
                var responseData = _mapper.Map<List<OrderViewModel>>(query);

                var paginationSet = new PaginationSet<OrderViewModel>()
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