using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuantitySystem.org.Controllers
{
    public class UnitsController : Controller
    {
        // GET: Units
        public ActionResult Index()
        {
            return View();
        }



        [Route("Units/{unit}")]
        public ActionResult Unit(string unit)
        {
            
            var u = Units.Unit.UnitTypes.First(x => x.Name == unit);
            Units.Unit cu = (Units.Unit)Activator.CreateInstance(u);
            return View(cu);
        }

    }
}