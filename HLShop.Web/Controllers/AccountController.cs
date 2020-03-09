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


        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        // POST: Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
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

        public ActionResult RecoverPassword(RecoverViewModel recoverVm)
        {
            return View(recoverVm);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng")]
        public async Task<ActionResult> RecoverPasswordPost(RecoverViewModel recoverVm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(recoverVm.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) 
                {
                    ModelState.AddModelError("email", "Email này chưa được đăng ký!");
                    return View("RecoverPassword", recoverVm);
                }

                var newPassword = "$123456$";
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(newPassword);
                await _userManager.UpdateAsync(user);

                ViewData["SuccessMsg"] = "Lấy lại mật khẩu thành công!";

                // send response mail
                string mailContent = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/recoverPassword.html"));
                mailContent = mailContent.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");
                mailContent = mailContent.Replace("{{Username}}", user.UserName);
                mailContent = mailContent.Replace("{{Password}}", newPassword);

                var toEmail = user.Email;
                string mailSubject = "Lấy lại mật khẩu tại hulasports.com";
                MailHelper.SendMail(toEmail, mailSubject, mailContent);
            }
            return View("RecoverPassword", recoverVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // Helper for External Login
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}