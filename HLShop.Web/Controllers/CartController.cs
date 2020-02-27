using HLShop.Common;
using HLShop.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.Mappings;

namespace HLShop.Web.Controllers
{
    public class CartController : Controller
    {
        private IProductService _productService;
        private IMapper _mapper;
        public CartController(IProductService productService)
        {
            this._productService = productService;
            this._mapper = AutoMapperConfiguration.Configure();
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
                        item.Quantity = quantity;
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
        public JsonResult Update(string cartData)
        {
            var cartVm = new JavaScriptSerializer().Deserialize<List<CartViewModel>>(cartData);
            var cartSession = (List<CartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartVm)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<CartViewModel>();
            return Json(new
            {
                status = true
            });
        }
    }
}