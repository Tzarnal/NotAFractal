using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotAFractal.Models.ViewModels
{
    public class NodeViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; } //What get displayed in the link
        public string Name { get; set; } //What gets displayed below the title when the node is open
        
    }
}