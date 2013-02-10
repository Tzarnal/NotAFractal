using NotAFractal.Models.ViewModels;
using System.Web.Mvc;

namespace NotAFractal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var tnode = new NodeViewModel {Id = 1,
                Title = "Not A Fractal", 
                Name = "A Limited Subset of Everything"};
            
            return View(tnode);
        }

    }
}
