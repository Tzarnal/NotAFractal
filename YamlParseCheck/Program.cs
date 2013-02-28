using System;
using System.Collections.Generic;
using NotAFractal.Data;
using System.IO;
using NotAFractal.Models;

namespace YamlParseCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";

            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                path = args[0];
            }
            else
            {
                path = Directory.GetCurrentDirectory();
            }

            var nodes = new Dictionary<string, FractalNode>();
            var generators = new Dictionary<string, DataGenerator>();

            try
            {
                nodes = YamlToNodeParser.ParseNodes(path + @"/FractalNodes");
               generators = YamlToDataGeneratorParser.ParseGenerators(path + @"/DataGenerators");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            
            if(nodes.Count == 0)
            {
                Console.WriteLine("Did not find any Fractal Nodes");
            }
            else
            {
                Console.WriteLine("Found {0} Fractal Nodes", nodes.Count);
            }

            if (generators.Count == 0)
            {
                Console.WriteLine("Did not find any Generators");
            }
            else
            {
                Console.WriteLine("Found {0} Generators ", generators.Count);
            }

            CheckNodes(nodes);
        }

        static void CheckNodes(Dictionary<string, FractalNode> nodes)
        {
            foreach (var fractalNode in nodes)
            {
                if (fractalNode.Value.ChoiceNodes != null)
                {
                    foreach (var choicenode in fractalNode.Value.ChoiceNodes)
                    {
                        foreach (var key in choicenode.WeightedStrings.Keys)
                        {
                            if (!nodes.ContainsKey(key))
                                Console.WriteLine("Could not find {0} referenced in the ChoiceNodes of {1}", key, fractalNode.Key);
                        }
                    }
                }
                
                if(fractalNode.Value.Nodes != null)
                {
                    foreach (var node in fractalNode.Value.Nodes)
                    {
                        if (!nodes.ContainsKey(node.Type))
                            Console.WriteLine("Could not find {0} referenced in the Nodes of {1}", node.Type, fractalNode.Key);
                    }                    
                }
            }
        }
    }
}
