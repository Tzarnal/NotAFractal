using System;
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
        
        public ActionResult Random()
        {
            var random = new Random();
            return View("Index",_nodes.BuildNodeViewModel(random.Next(1,int.MaxValue), "RootNode"));
        }

        public ActionResult Root(string type, int seed)
        {
            return View("Index", _nodes.BuildNodeViewModel(seed,type));
        }
    }
}
