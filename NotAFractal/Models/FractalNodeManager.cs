using System.Collections.Generic;
using NotAFractal.Data;
using NotAFractal.Models.Builders;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models
{
    public class FractalNodeManager
    {
        //YamlNodeManager is a singleton, to ensure the potentially large set of nodes only exists once.
        private static FractalNodeManager _instance;
        private Dictionary<string,FractalNode> _nodeList;
        private NodeViewModelBuilder _nodeViewModelBuilder;


        public static FractalNodeManager Instance
        {
            get { return _instance ?? (_instance = new FractalNodeManager()); }
        }
            
        private FractalNodeManager()
        {
            _nodeList = YamlToNodeParser.ParseNodes();
            _nodeViewModelBuilder  = new NodeViewModelBuilder();
        }

        public NodeViewModel BuildNodeViewModel(int seed, string type)
        {
            if (!_nodeList.ContainsKey(type))
            {
                return _nodeViewModelBuilder.ExceptionNode("Could not find this Node Type:" + type);
            }

            return _nodeViewModelBuilder.Build(seed, _nodeList[type]);
        }

        public NodeViewModel BuildNodeViewModelStub(int seed, string type)
        {
            if (!_nodeList.ContainsKey(type))
            {
                return _nodeViewModelBuilder.ExceptionNode("Could not find this Node Type: " + type);
            }

            return _nodeViewModelBuilder.BuildStub(seed, _nodeList[type]);
        }
        
        public NodeInformationViewModel BuildNodeInformationViewModel(int seed, string type)
        {
            if (!_nodeList.ContainsKey(type))
            {
                throw new KeyNotFoundException("Could not find this Node Type");
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