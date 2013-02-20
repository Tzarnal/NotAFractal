using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class HomeController : Controller
    {
        private FractalNodeManager _nodes = FractalNodeManager.Instance;
        
        public ActionResult Index()
        {            
            return View(_nodes.BuildNodeViewModel(1,"Root"));            
        }
        
        public ActionResult Root(string type, int seed)
        {
            return View("Index", _nodes.BuildNodeViewModel(seed,type));
        }
    }
}
