using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotAFractal.Models
{
    public class FractalNode
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public string SidebarText { get; set; }
        public string SidebarUrl { get; set; }
        public string SidebarTitle { get; set; }

        public List<FractalNodeWeighted> Nodes { get; set; }
        public List<WeightedChoiceEntry> ChoiceNodes { get; set; }

        public List<string> PickChoiceNodes(int seed)
        {
            var random = new Random(seed);

            return ChoiceNodes.Select(dataGeneratorEntry => dataGeneratorEntry.Generate(random.Next(1, int.MaxValue))).ToList();
        }
    }
}