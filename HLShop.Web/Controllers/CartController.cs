using AutoMapper;
using HLShop.Common;
using HLShop.Service;
using HLShop.Web.App_Start;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HLShop.Model.Models;
using HLShop.Web.Infrastructure.Extensions;
using Microsoft.AspNet.Identity;

namespace HLShop.Web.Controllers
{
    public class CartController : Controller
    {
        private IProductService _productService;
        private IOrderService _orderService;
        private ApplicationUserManager _userManager;
        private IMapper _mapper;

        public CartController(IProductService productService, IOrderService orderService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._orderService = orderService;
            this._mapper = AutoMapperConfiguration.Configure();
            this._userManager = userManager;
        }

        // GET: Cart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<CartViewModel>();
            }
            return View();
        }

        public ActionResult Checkout()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);

                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public JsonResult CreateOrder(string orderViewModel)
        {
            var orderVm = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNewModel = new Order();

            // gan gia tri cho orderNewModel
            orderNewModel.UpdateOrder(orderVm);
            if (Request.IsAuthenticated)
            {
                orderNewModel.CustomerId = User.Identity.GetUserId();
                orderNewModel.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<CartViewModel>)Session[CommonConstants.SessionCart];   
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                orderDetails.Add(detail);
            }

            _orderService.Create(orderNewModel, orderDetails);

            return Json(new
            {
                status = true
            });
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<CartViewModel>();
            }
            var cart = (List<CartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<CartViewModel>)Session[CommonConstants.SessionCart];
            if (cart == null)
            {
                cart = new List<CartViewModel>();
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity++;
                    }
                }
            }
            else
            {
                CartViewModel newItem = new CartViewModel();
                newItem.ProductId = productId;
                var product = _productService.GetById(productId);
                var productVm = _mapper.Map<ProductViewModel>(product);
                newItem.Product = productVm;
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int quantity)
        {
            var cart = (List<CartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cart)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity = quantity;
                }
            }
            Session[CommonConstants.SessionCart] = cart;

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<CartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }


        [HttpPost]
        public JsonResult DeleteAllItem()
        {
            Session[CommonConstants.SessionCart] = new List<CartViewModel>();
            return Json(new
            {
                status = true
            });
        }
    }
}