using System.Collections.Generic;
using NotAFractal.Models;

namespace NotAFractal.Data
{
    public class YamlToNodeParser
    {
        public static List<YamlNode> ParseNodes()
        {
            var nodes = new List<YamlNode>();
            
            nodes.Add(ParseNode("bla"));

            return nodes;
        }

        private static YamlNode ParseNode(string fileName)
        {
            return new YamlNode();
        }
    }
}