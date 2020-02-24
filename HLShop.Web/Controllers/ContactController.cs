using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using HLShop.Service;
using HLShop.Web.Mappings;
using HLShop.Web.Models;

namespace HLShop.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactDetailService _cdService;

        public ContactController(IContactDetailService cdService)
        {
            this._cdService = cdService;
        }

        // GET: Contact
        public ActionResult Index()
        {
            var contactDetailModel = _cdService.GetDefaultContact();
            IMapper mapper = AutoMapperConfiguration.Configure();
            var contactDetailVm = mapper.Map<ContactDetailViewModel>(contactDetailModel);
            return View(contactDetailVm);
        }
    }
}