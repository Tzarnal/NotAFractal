using System.Collections.Generic;

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

    }
}