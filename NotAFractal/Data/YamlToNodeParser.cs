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
        private static string _curFileName;
        
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
            _curFileName = fileName;

            try
            {
                using (var input = new StreamReader(fileName))
                {
                    var yaml = new YamlStream();
                    yaml.Load(input);

                    return ParseYamlFile(yaml);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Opening/Parsing: " + fileName);
                Debug.WriteLine(e.Message);
                throw;
            } 
        }

        private static FractalNode ParseYamlFile(YamlStream yaml)
        {
            var node = new FractalNode();
  
            try
            {
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                //the mapping is index with yamlscalars and not strings
                //So we need to build a new yamlscalarnode for every named entry
                //Before checking them and adding them ot the node.

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

                //Check for a nodes node, if it exists iterate over all its children
                var nodesScalar = new YamlScalarNode("Nodes");
                if (mapping.Children.Keys.Contains(nodesScalar))
                {                    
                    var subnodes = (YamlSequenceNode) mapping.Children[nodesScalar];
                    
                    foreach (YamlMappingNode subnode in subnodes)
                    {
                        var nodeWeighted = new FractalNodeWeighted();

                        //check for a type entry, and if it exists add it, Type is the one mandatory entry
                        var typeScalar = new YamlScalarNode("Type");
                        if(subnode.Children.Keys.Contains(typeScalar))
                        {
                            nodeWeighted.Type = subnode.Children[typeScalar].ToString();
                        }
                        else
                        {
                            break; //if there is no Type this entry isn't meaningful
                        }

                        //Parsing into an int can be tricky and users are likely to put in something that won't parse so we need more sanity checks here
                        var chanceScalar = new YamlScalarNode("PercentageChance");
                        if (subnode.Children.Keys.Contains(chanceScalar))
                        {
                            var amount = 1;

                            if (!int.TryParse(subnode.Children[chanceScalar].ToString(), out amount))
                            {
                                Debug.WriteLine("Could not parse int from subnode: {0} defaulting to 1", subnode);
                            }

                            if( amount > 0 && amount <= 100)
                            {
                                nodeWeighted.PercentageChance = amount;    
                            }
                            else
                            {
                                nodeWeighted.PercentageChance = 100;
                            }
                            
                        }
                        else
                        {
                            nodeWeighted.PercentageChance = 100;
                        }
                            
                        

                        //need to do things a bit more complex because minimum may not exceed maximum                                                        
                        var maxScalar = new YamlScalarNode("MaxAmount");
                        if (subnode.Children.Keys.Contains(maxScalar))
                        {
                            var amount = 1;

                            if (!int.TryParse(subnode.Children[maxScalar].ToString(), out amount))
                            {
                                Debug.WriteLine("Could not parse int from subnode: {0} defaulting to 1", subnode);
                            }

                            nodeWeighted.MaxAmount = amount;
                        }
                        else
                        {
                            nodeWeighted.MaxAmount = 1;
                        }

                        var minScalar = new YamlScalarNode("MinAmount");
                        if (subnode.Children.Keys.Contains(minScalar))
                        {
                            var amount = 1;

                            if (!int.TryParse(subnode.Children[minScalar].ToString(), out amount))
                            {
                                Debug.WriteLine("Could not parse int from subnode: {0} defaulting to 1", subnode);
                            }

                            nodeWeighted.MinAmount = (amount > nodeWeighted.MaxAmount) ? nodeWeighted.MaxAmount : amount;
                        }
                        else
                        {
                            nodeWeighted.MinAmount = nodeWeighted.MaxAmount;
                        }

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
                            var weight = 1;

                            if (!int.TryParse(subnodeData[1].ToString(), out weight))
                            {
                                Debug.WriteLine("Could not parse int from subnode: {0} defaulting to 1", subnode);
                            }

                            entry.TotalWeight += weight;
                            entry.WeightedStrings.Add(text,weight);
                        }

                        node.ChoiceNodes.Add(entry);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error parsing: {0}", _curFileName);
                Debug.WriteLine(e);
                return null;
            }

            return node;
        }
    }
}