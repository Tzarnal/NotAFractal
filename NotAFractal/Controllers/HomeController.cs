using NotAFractal.Models;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class HomeController : Controller
    {
        private ModelManager _nodes = ModelManager.Instance;
        
        public ActionResult Index()
        {            
            return View(_nodes.BuildNodeViewModel(1,"RootNode"));            
        }
        
        public ActionResult Root(string type, int seed)
        {
            return View("Index", _nodes.BuildNodeViewModel(seed,type));
        }
    }
}
