using System;
using System.Collections.Generic;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models.Builders
{
    public class NodeViewModelBuilder
    {
        public NodeViewModel Build(int seed, FractalNode type)
        {
            var random = new Random(seed);
            var modelManager = ModelManager.Instance;

            var nodeViewModel = new NodeViewModel
            {
                Title = type.Title,
                Name = type.Name,
                Text = type.Text,

                Seed = seed,
                Type = type.Type,
            };

            if (type.Nodes == null)
                return nodeViewModel;

            foreach (var weightedNode in type.Nodes)
            {
                if(random.Next(0,100) <=  weightedNode.PercentageChance)
                {
                    var count = random.Next(weightedNode.MinAmount, weightedNode.MaxAmount);
                    
                    while( count > 0)
                    {
                        if( nodeViewModel.ChildNodes == null)
                            nodeViewModel.ChildNodes = new List<NodeViewModel>();
                        
                        nodeViewModel.ChildNodes.Add(modelManager.BuildNodeViewModelStub(random.Next(1,int.MaxValue),weightedNode.Type));
                        count--;
                    }
                }
            }

            return nodeViewModel;
        }

        public NodeViewModel BuildStub(int seed, FractalNode type)
        {
            var random = new Random(seed);
            var modelManager = ModelManager.Instance;

            var nodeViewModel = new NodeViewModel
            {
                Title = modelManager.ProcessDataGeneratorSymbols(random.Next(0,int.MaxValue), type.Title),

                Seed = seed,
                Type = type.Type,
            };

            return nodeViewModel;
        }

        public NodeViewModel ExceptionNode(string message)
        {            
            var nodeViewModel = new NodeViewModel
            {
                Title = message,
                Seed = 1,
                Type = "ExceptionNode",
            };

            return nodeViewModel;
        }
    }
}