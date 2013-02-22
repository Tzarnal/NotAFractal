using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using NotAFractal.Models;
using YamlDotNet.RepresentationModel;

namespace NotAFractal.Data
{
    public class YamlToNodeParser
    {
        public static Dictionary<string, FractalNode> ParseNodes()
        {
            var path = HostingEnvironment.MapPath(@"~/Data/FractalNodes");

            if(path == null)
                throw new FileNotFoundException();

            var nodes = new Dictionary<string, FractalNode>();
            var nodeFiles = Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories);

            foreach (var nodeFile in nodeFiles)
            {                                
                var node = ReadFile(nodeFile);                
                var nodeType = Path.GetFileNameWithoutExtension(nodeFile);

                if( node != null)
                {
                    node.Type = nodeType;
                    // ReSharper disable AssignNullToNotNullAttribute
                    nodes.Add(nodeType, node);
                    // ReSharper restore AssignNullToNotNullAttribute         
                }
       
            }

            return nodes;
        }

        private static FractalNode ReadFile(string fileName)
        {           
            try
            {
                using (var input = new StreamReader(fileName))
                {
                    return ParseYamlFile(input);
                }
            }
            catch (Exception)
            {                
                Debug.WriteLine("Error Opening {0}",fileName );

                throw;
            }
        }

        private static FractalNode ParseYamlFile(StreamReader input)
        {
            var node = new FractalNode();
            var yaml = new YamlStream();

            try
            {
                yaml.Load(input);
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                var titleScalar = new YamlScalarNode("Title");
                if (mapping.Children.Keys.Contains(titleScalar))
                    node.Title = mapping.Children[titleScalar].ToString();

                var nameScalar = new YamlScalarNode("Name");
                if (mapping.Children.Keys.Contains(nameScalar))
                    node.Name = mapping.Children[nameScalar].ToString();

                var textScalar = new YamlScalarNode("Text");
                if (mapping.Children.Keys.Contains(textScalar))
                    node.Text = mapping.Children[textScalar].ToString();

                var sideTitleScalar = new YamlScalarNode("SidebarTitle");
                if (mapping.Children.Keys.Contains(sideTitleScalar))
                    node.SidebarTitle = mapping.Children[sideTitleScalar].ToString();

                var sideTextScalar = new YamlScalarNode("SidebarText");
                if (mapping.Children.Keys.Contains(sideTextScalar))
                    node.SidebarText = mapping.Children[sideTextScalar].ToString();

                var sideUrlScalar = new YamlScalarNode("SidebarURL");
                if (mapping.Children.Keys.Contains(sideUrlScalar))
                    node.SidebarUrl = mapping.Children[sideUrlScalar].ToString();

                var nodesScalar = new YamlScalarNode("Nodes");
                if (mapping.Children.Keys.Contains(nodesScalar))
                {
                    var subnodes = (YamlSequenceNode) mapping.Children[nodesScalar];
                    
                    foreach (YamlMappingNode subnode in subnodes)
                    {
                        var nodeWeighted = new FractalNodeWeighted();

                        var typeScalar = new YamlScalarNode("Type");
                        if(subnode.Children.Keys.Contains(typeScalar))
                        {
                            nodeWeighted.Type = subnode.Children[typeScalar].ToString();
                        }
                        else
                        {
                            break; //if there is no Type this entry isn't meaningful
                        }

                        var chanceScalar = new YamlScalarNode("PercentageChance");
                        nodeWeighted.PercentageChance = subnode.Children.Keys.Contains(chanceScalar) ? int.Parse(subnode.Children[chanceScalar].ToString()) : 100;
                                                    
                        var minScalar = new YamlScalarNode("MinAmount");
                        nodeWeighted.MinAmount = subnode.Children.Keys.Contains(minScalar) ? int.Parse(subnode.Children[minScalar].ToString()) : 1;

                        var maxScalar = new YamlScalarNode("MaxAmount");
                        nodeWeighted.MaxAmount = subnode.Children.Keys.Contains(maxScalar) ? int.Parse(subnode.Children[maxScalar].ToString()) : 1;
                        
                        if(node.Nodes == null)
                        {
                            node.Nodes = new List<FractalNodeWeighted>();
                        }

                        node.Nodes.Add(nodeWeighted);
                    }
                }

                var choiceNodesScalar = new YamlScalarNode("ChoiceNodes");
                if(mapping.Children.Keys.Contains(choiceNodesScalar))
                {
                    var choiceNodes = (YamlSequenceNode) mapping.Children[choiceNodesScalar];
                    node.ChoiceNodes = new List<WeightedChoiceEntry>();

                    foreach (YamlSequenceNode choiceNode in choiceNodes)
                    {
                        var entry = new WeightedChoiceEntry {WeightedStrings = new Dictionary<string, int>()};

                        foreach (YamlSequenceNode subnode in choiceNode)
                        {
                            var subnodeData = subnode.Children;

                            var text = subnodeData[0].ToString();
                            var weight = int.Parse(subnodeData[1].ToString());

                            entry.TotalWeight += weight;
                            entry.WeightedStrings.Add(text,weight);
                        }

                        node.ChoiceNodes.Add(entry);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            return node;
        }
    }
}