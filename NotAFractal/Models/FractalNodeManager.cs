using System.Collections.Generic;
using NotAFractal.Data;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models
{
    public class FractalNodeManager
    {
        //YamlNodeManager is a singleton, to ensure the potentially large set of nodes only exists once.
        private static FractalNodeManager _instance;
        private Dictionary<string,FractalNode> _nodeList;

        public static FractalNodeManager Instance
        {
            get { return _instance ?? (_instance = new FractalNodeManager()); }
        }
            
        private FractalNodeManager()
        {
            _nodeList = YamlToNodeParser.ParseNodes();
        }

        public NodeViewModel BuildNodeViewModel(int seed, string type)
        {
            if (!_nodeList.ContainsKey(type))
            {
                throw new KeyNotFoundException("Could not find the specified Node Type.");
            }

            var fractalNode = _nodeList[type];
            var nodeViewModel = new NodeViewModel
                           {
                               Title = fractalNode.Title,
                               Name = fractalNode.Name,                               
                               Text = fractalNode.Text,
                               
                               Seed = seed,
                               Type = type,
                           };

            return nodeViewModel;
        }

        public NodeInformationViewModel BuildNodeInformationViewModel(int seed, string type)
        {
            if (!_nodeList.ContainsKey(type))
            {
                throw new KeyNotFoundException("Could not find the specified Node Type.");
            }

            var fractalNode = _nodeList[type];
            var nodeInformationViewModel = new NodeInformationViewModel
            {
                Title = fractalNode.SidebarTitle,                
                Text = fractalNode.SidebarText,
                Link = fractalNode.SidebarUrl,                
                
                Seed = seed,
                Type = type,
            };

            return nodeInformationViewModel;
        }
    }
}