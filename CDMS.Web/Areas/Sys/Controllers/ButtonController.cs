using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDMS.Entity;
using CDMS.Service;
using CDMS.Utility;

namespace CDMS.Web.Areas.Sys.Controllers
{
    public class ButtonController : BaseController
    {
        readonly IButtonService buttonService;

        public ButtonController(IButtonService ibs)
        {
            buttonService = ibs;
        }

        // GET: Sys/Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int pid)
        {
            ViewBag.PID = pid;
            return View();
        }

        public ActionResult Form(int? id)
        {
            int buttonId = id.HasValue ? id.Value : 0;
            if (buttonId > 0)
            {
                ViewBag.ButtonJson = JsonHelper.ToJson(buttonService.Get(buttonId));
            }
            return View();
        }

        [HttpPost]
        public ActionResult Save(Button model)
        {
            if (!string.IsNullOrEmpty(model.IMG))
                model.IMG = Uri.UnescapeDataString(model.IMG);
            var result = buttonService.Save(model);
            return Json(result);
        }
        [HttpPost]
        public ActionResult GetList(PaginationIn p)
        {
            var list = buttonService.GetList(p);
            return Json(list);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var result = buttonService.Delete(ids);
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddButtons(int pid, int[] ids)
        {
            var result = buttonService.AddButtons(pid, ids);
            return Json(result);
        }
    }
}