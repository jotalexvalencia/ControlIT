using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace ControlIT.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/  
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Vehiculos()
        {
            return View();
        }
    }
}
