using System;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models
{
    public class StubModel
    {
        public static NodeViewModel GetRootNode(int seed)
        {
            var node = GetNode(1, "Root");
            node.Title = "Not A Fractal";
            node.Name = "A limited subset of everything";
            node.Type = "Root";

            return node;
        }
 
        public static NodeViewModel GetNode(int seed, string type)
        {
            var random = new Random(seed);
            var node = new NodeViewModel();
            
            var childCount = random.Next(1, 10);

            for (int i = 0; i < childCount; i++)
            {
                var tnode = new NodeViewModel
                                {
                                    Title = i.ToString(),
                                    Seed = random.Next(1,int.MaxValue),
                                    Type = type,
                                };
                node.ChildNodes.Add(tnode);
            }

            return node;
        }
    }
}