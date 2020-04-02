using AutoMapper;
using HLShop.Common;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.App_Start;
using HLShop.Web.Infrastructure.Extensions;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HLShop.Web.Controllers
{
    public class CartController : Controller
    {
        private IProductService _productService;
        private ICartService _cartService;
        private IOrderService _orderService;
        private ApplicationUserManager _userManager;
        private IMapper _mapper;

        public CartController(IProductService productService, ICartService cartService, IOrderService orderService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._cartService = cartService;
            this._orderService = orderService;
            this._mapper = AutoMapperConfiguration.Configure();
            this._userManager = userManager;
        }

        // GET: Cart
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var cartDb = _cartService.GetCartItemByUser(userId);
            }
            else
            {
                if (Session[CommonConstants.SessionCart] == null)
                {
                    Session[CommonConstants.SessionCart] = new List<CartViewModel>();
                }
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
                var userId = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();

                orderNewModel.CustomerId = userId;
                orderNewModel.CreatedBy = userName;

                var cartDb = _cartService.GetCartItemByUser(userId);
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var item in cartDb)
                {
                    var detail = new OrderDetail();
                    detail.ProductID = item.ProductId;
                    detail.Quantity = item.Quantity;

                    var product = _productService.GetById(item.ProductId);
                    // giam so san pham con lai di 
                    _productService.UpdateQuantity(product, item.Quantity);

                    detail.ProductName = product.Name;
                    detail.ProductImage = product.Image;
                    detail.ProductPrice = product.Price;
                    orderDetails.Add(detail);
                }
                _orderService.Create(orderNewModel, orderDetails);
            }
            else
            {
                var cart = (List<CartViewModel>)Session[CommonConstants.SessionCart];
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var item in cart)
                {
                    var detail = new OrderDetail();
                    detail.ProductID = item.ProductId;
                    detail.Quantity = item.Quantity;

                    var product = _productService.GetById(item.ProductId);
                    // giam so san pham con lai di 
                    _productService.UpdateQuantity(product, item.Quantity);

                    detail.ProductName = product.Name;
                    detail.ProductImage = product.Image;
                    detail.ProductPrice = product.Price;
                    orderDetails.Add(detail);
                }
                _orderService.Create(orderNewModel, orderDetails);
            }

            return Json(new
            {
                status = true
            });
        }

        public JsonResult GetAll()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var cartDb = _cartService.GetCartItemByUser(userId);
                var cartVm = _mapper.Map<List<CartViewModel>>(cartDb);
                foreach (var item in cartVm)
                {
                    var product = _productService.GetById(item.ProductId);
                    var productVm = _mapper.Map<ProductViewModel>(product);
                    item.Product = productVm;
                }
                return Json(new
                {
                    data = cartVm,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
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
        }

        [HttpPost]
        public JsonResult Add(int productId, int quantity)
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var cartDb = _cartService.GetCartItemByUser(userId);

                if (cartDb.Count()==0)
                {
                    var newCartItem = new Cart();
                    newCartItem.UserId = userId;
                    newCartItem.ProductId = productId;
                    newCartItem.Quantity = quantity;
                    _cartService.Add(newCartItem);
                }
                else
                {
                    var check = false;
                    foreach (var item in cartDb)
                    {
                        if (item.ProductId == productId)
                        {
                            check = true;
                            item.Quantity = item.Quantity + quantity;
                            _cartService.Update(item);
                        }
                    }

                    if (check == false)
                    {
                        var newCartItem = new Cart();
                        newCartItem.UserId = userId;
                        newCartItem.ProductId = productId;
                        newCartItem.Quantity = quantity;
                        _cartService.Add(newCartItem);
                    }
                }
            }
            else
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
                            item.Quantity = item.Quantity + quantity;
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
                    newItem.Quantity = quantity;
                    cart.Add(newItem);
                }

                Session[CommonConstants.SessionCart] = cart;
            }
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int quantity)
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var cartDb = _cartService.GetCartItemByUser(userId);
                foreach (var item in cartDb)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity = quantity;
                        _cartService.Update(item);
                    }
                }
            }
            else
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
            }

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var cartDb = _cartService.GetCartItemByUser(userId);
                foreach (var item in cartDb)
                {
                    if (item.ProductId == productId)
                    {
                        _cartService.Delete(item);
                    }
                }
                return Json(new
                {
                    status = true
                });
            }
            else
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
            }

            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult DeleteAllItem()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                _cartService.DeleteAllItem(userId);
            }
            else
            {
                Session[CommonConstants.SessionCart] = new List<CartViewModel>();
            }
            return Json(new
            {
                status = true
            });
        }
    }
}