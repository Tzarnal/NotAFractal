using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotAFractal.Models.ViewModels
{
    public class NodeViewModel
    {
        public int Id { get; set; } //To be used in the id's of the html elements
        public int Seed { get; set; } //The seed this element was generated with
        
        public string Title { get; set; } //What get displayed in the link
        public string Name { get; set; } //What gets displayed below the title when the node is open

        public List<NodeViewModel> ChildNodes { get; set; }

    }
}