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
            node.Seed = 1;

            return node;
        }

        public static NodeViewModel GetRootNode(int seed, string type)
        {
            var node = GetNode(seed, type);

            node.Title = "Root";
            node.Name = "Custom Root Node " + seed;
            node.Seed = seed;
            node.Type = type;

            return node;
        }

        public static NodeViewModel GetNode(int seed, string type)
        {
            var random = new Random(seed);
            var node = new NodeViewModel();
            
            var childCount = random.Next(0, 10);

            for (int i = 0; i < childCount; i++)
            {
                var tnode = new NodeViewModel
                                {
                                    Title = i.ToString(),
                                    Seed = random.Next(1,int.MaxValue),
                                    Type = "Node",
                                };
                node.ChildNodes.Add(tnode);
            }

            return node;
        }

        public static object GetNodeInformation(int seed, string type)
        {
            if(type == "Root")
            {
                return new NodeInformationViewModel
                {
                    Title = "Not A Fractal",
                    Text = "Not a Fractal is named this way because a 2012 study found that the universe is probably not a fractal. And thats a marginally more clever name then Universe.",
                    Link = "http://github.com/Xesyto/NotAFractal",
                    Seed = seed,
                    Type = type
                };    
            }

            return new NodeInformationViewModel
            {
                Title = "A Node",
                Text = "This node has a type of: " + type + " and was generated with a seed of: " + seed,
                Link = "http://en.wikipedia.org/wiki/Node_%28computer_science%29",
                Seed = seed,
                Type = type
            };    

                   
        }
    }
}