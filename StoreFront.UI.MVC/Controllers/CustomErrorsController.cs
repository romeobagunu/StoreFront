using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreFront.UI.MVC.Controllers
{
    public class CustomErrorsController : Controller
    {
        // GET: CustomErrors
        public ActionResult Error404()
        {
            return View();
        }
    }
}