using System.Security.Claims;
using HLShop.Model.Models;
using HLShop.Web.App_Start;
using HLShop.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using HLShop.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace HLShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            RegisterViewModel registerVm = new RegisterViewModel();
            return View(registerVm);
        }

        // GET: Account
        public ActionResult Login(LoginViewModel loginVm, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(loginVm);
        }

        [HttpPost]
        public async Task<ActionResult> LoginPost(LoginViewModel loginVm, string returnUrl)
        {
            ApplicationUser user = await _userManager.FindAsync(loginVm.Username, loginVm.Password);
            if (user != null)
            {
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                // ClaimsIdentity: chua thong tin dang nhap, quyen`, ... (luu trong cookie)
                ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationProperties props = new AuthenticationProperties();
                props.IsPersistent = loginVm.RememberMe;
                authenticationManager.SignIn(props, identity);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("error","Tên đăng nhập hoặc mật khẩu không đúng!");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View("Login", loginVm);
        }

        [HttpGet]
        public ActionResult RegisterGet()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng")]
        public async Task<ActionResult> RegisterPost(RegisterViewModel registerVm)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(registerVm.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email này đã được đăng ký!");
                    return View("Index", registerVm);
                }

                var userByUsername = await _userManager.FindByNameAsync(registerVm.Username);
                if (userByUsername != null)
                {
                    ModelState.AddModelError("username", "Tài khoản này đã tồn tại!");
                    return View("Index", registerVm);
                }

                var user = new ApplicationUser()
                {
                    Fullname = registerVm.Fullname,
                    BirthDay = registerVm.Birthday,
                    Email = registerVm.Email,
                    EmailConfirmed = true,
                    Address = registerVm.Address,
                    PhoneNumber = registerVm.PhoneNumber,
                    UserName = registerVm.Username
                };

                await _userManager.CreateAsync(user, registerVm.Password);

                var clientUser = await _userManager.FindByEmailAsync(registerVm.Email);
                if (clientUser != null)
                {
                    await _userManager.AddToRolesAsync(clientUser.Id, new string[] { "User" });
                }

                ViewData["SuccessMsg"] = "Đăng ký tài khoản thành công!";

                // send response mail
                string mailContent = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newUser.html"));
                mailContent = mailContent.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink")+"dang-nhap.html");
                mailContent = mailContent.Replace("{{Username}}", clientUser.UserName);

                var toEmail = clientUser.Email;
                string mailSubject = "Đăng ký tài khoản thành công";
                MailHelper.SendMail(toEmail, mailSubject, mailContent);
            }

            return View("Index", registerVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}