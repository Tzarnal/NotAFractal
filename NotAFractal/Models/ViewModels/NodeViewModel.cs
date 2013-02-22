using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotAFractal.Models.ViewModels
{
    public class NodeViewModel
    {        
        public int Seed { get; set; } //The seed this element was generated with
        public string Type { get; set; } //The type of node this is ( galaxy, starsystem, star, planet, etc)
        public bool EmptyNode { get; set; } //Some nodes only ever display Title data, those are empty nodes and don't need an expansion icon

        public string Title { get; set; } //What get displayed in the link
        public string Name { get; set; } //What gets displayed below the title when the node is open
        public string Text { get; set; } //Generic text to display before the nodes

        public List<NodeViewModel> ChildNodes { get; set; }

        public NodeViewModel()
        {
            ChildNodes = new List<NodeViewModel>();
        }
    }
}