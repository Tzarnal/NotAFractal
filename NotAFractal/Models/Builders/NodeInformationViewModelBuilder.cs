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
            var nodeInformationViewModel = new NodeInformationViewModel
            {
                Title = type.SidebarTitle,
                Text = type.SidebarText,
                Link = type.SidebarUrl,

                Seed = seed,
                Type = type.Type,
            };

            return nodeInformationViewModel;
        }
    }
}