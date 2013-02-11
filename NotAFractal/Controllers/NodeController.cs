using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class NodeController : Controller
    {
    
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetNode(string type, int seed)
        {
            return View(StubModel.GetNode(seed, type));
        }

        public ActionResult GetNodeInformation(string type, int seed)
        {
            return View(StubModel.GetNodeInformation(seed, type));
        }
    }
}
