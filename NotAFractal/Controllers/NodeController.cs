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

        public ActionResult GetNode(string id, int seed)
        {
            return View(StubModel.GetNode(seed,id));
        }

        public ActionResult GetNodeInformation(string id, int seed)
        {
            return View(StubModel.getNodeInformation(seed, id));
        }
    }
}
