using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models.Builders
{
    public class NodeInformationViewModelBuilder
    {
        public NodeInformationViewModel Build(int seed, FractalNode type)
        {
            var random = new Random(seed);
            var modelManager = ModelManager.Instance;
            var dataGeneratorSeed = random.Next(1, int.MaxValue);
            
            var nodeInformationViewModel = new NodeInformationViewModel
            {
                Title = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.SidebarTitle),
                Text = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.SidebarText),
                Link = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.SidebarUrl),

                Seed = seed,
                Type = type.Type,
            };

            return nodeInformationViewModel;
        }
    }
}