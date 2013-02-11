using System.Collections.Generic;
using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(StubModel.GetRootNode(1));
        }

    }
}
