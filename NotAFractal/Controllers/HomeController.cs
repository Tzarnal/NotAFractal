using System.Collections.Generic;
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
            var tnode = new NodeViewModel
                            {
                                Id = 1,
                                Title = "Not A Fractal",
                                Name = "A Limited Subset of Everything",
                                Seed = 1,
                                ChildNodes = new List<NodeViewModel>()
                            };

            var childOne = new NodeViewModel
                               {
                                   Id = 2,
                                   Title = "Galaxy",
                                   Seed = 12
                               };

            var childTwo = new NodeViewModel
                                {
                                    Id = 3,
                                    Title = "Galaxy",
                                    Seed = 13
                                };
            
            tnode.ChildNodes.Add(childOne);
            tnode.ChildNodes.Add(childTwo);

            return View(tnode);
        }

    }
}
