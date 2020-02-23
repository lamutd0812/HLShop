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
    public class PageController : Controller
    {
        private IPageService _pageService;

        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }

        // GET: Page
        public ActionResult Index(string alias)
        {
            IMapper mapper = AutoMapperConfiguration.Configure();
            var page = _pageService.GetByAlias(alias);
            var pageVm = mapper.Map<PageViewModel>(page);
            return View(pageVm);
        }
    }
}