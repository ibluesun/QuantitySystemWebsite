using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuantitySystem.org.Controllers
{
    public class QuantitiesController : Controller
    {
        // GET: Quantities
        public ActionResult Index()
        {
            return View();
        }


        [Route("Quantities/{quantity}")]
        public ActionResult Quantity(string quantity)
        {
            ViewBag.Quantity = quantity;
            return View();
        }
    }
}