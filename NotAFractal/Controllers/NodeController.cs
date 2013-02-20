using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class NodeController : Controller
    {
        private FractalNodeManager _nodes = FractalNodeManager.Instance;

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetNode(string type, int seed)
        {
            return View(_nodes.BuildNodeViewModel(seed,type));
        }

        public ActionResult GetNodeInformation(string type, int seed)
        {
            return View(_nodes.BuildNodeInformationViewModel(seed,type));
        }
    }
}
