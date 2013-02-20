using System.Collections.Generic;
using NotAFractal.Data;

namespace NotAFractal.Models
{
    public class YamlNodeManager
    {
        //YamlNodeManager is a singleton, to ensure the potentially large set of nodes only exists once.
        private static YamlNodeManager _instance;

        private List<YamlNode> _nodeList;

        public static YamlNodeManager Instance
        {
            get { return _instance ?? (_instance = new YamlNodeManager()); }
        }
    
        
        private YamlNodeManager()
        {
            _nodeList = YamlToNodeParser.ParseNodes();
        }

    }

}