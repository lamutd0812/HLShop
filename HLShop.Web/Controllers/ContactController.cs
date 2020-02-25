using System.Text;
using AutoMapper;
using HLShop.Model.Models;
using HLShop.Service;
using HLShop.Web.Infrastructure.Extensions;
using HLShop.Web.Mappings;
using HLShop.Web.Models;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using HLShop.Common;

namespace HLShop.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactDetailService _cdService;
        private IFeedbackService _feedbackService;
        private IMapper _mapper;

        public ContactController(IContactDetailService cdService, IFeedbackService feedbackService)
        {
            this._cdService = cdService;
            this._feedbackService = feedbackService;
            this._mapper = AutoMapperConfiguration.Configure();
        }

        // GET: Contact
        public ActionResult Index()
        {
            FeedbackViewModel feedbackVm = new FeedbackViewModel();
            feedbackVm.ContactDetail = getContactDetail();

            return View(feedbackVm);
        }

        // feed back
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "contactCaptcha", "Mã xác nhận không đúng")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackVm)
        {
            if (ModelState.IsValid)
            {
                Feedback newFeedback = new Feedback();
                newFeedback.UpdateFeedback(feedbackVm);
                _feedbackService.Create(newFeedback);
                _feedbackService.Save();

                ViewData["SuccessMsg"] = "Gửi phản hồi thành công!";

                // send response mail
                string mailContent = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/contactMailResponse.html"));
                mailContent = mailContent.Replace("{{Name}}", feedbackVm.Name);
                mailContent = mailContent.Replace("{{Email}}", feedbackVm.Email);
                mailContent = mailContent.Replace("{{Message}}", feedbackVm.Message);

                //var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                var toEmail = feedbackVm.Email;

                string mailSubject = "Trả lời phản hồi khách hàng:";
                MailHelper.SendMail(toEmail, mailSubject, mailContent);
            }
            //Reset infor in form after submit
            //ModelState.Clear();

            feedbackVm.ContactDetail = getContactDetail();
            return View("Index", feedbackVm);
        }

        private ContactDetailViewModel getContactDetail()
        {
            var contactDetailModel = _cdService.GetDefaultContact();
            var contactDetailVm = _mapper.Map<ContactDetailViewModel>(contactDetailModel);
            return contactDetailVm;
        }

    }
}