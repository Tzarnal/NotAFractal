using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(StubModel.GetRootNode(1));
        }
        

        public ActionResult Root(string type, int seed)
        {
            return View("Index", StubModel.GetRootNode(seed, type));
        }
    }
}
