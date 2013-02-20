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
            var path = HostingEnvironment.MapPath(@"~/Data");

            if(path == null)
                throw new FileNotFoundException();

            var nodes = new Dictionary<string, FractalNode>();
            var nodeFiles = Directory.GetFiles(path, "*.yaml");

            foreach (var nodeFile in nodeFiles)
            {                
                nodes.Add(Path.GetFileNameWithoutExtension(nodeFile),ReadFile(nodeFile));                
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
            catch (Exception e)
            {                
                Debug.WriteLine("Error Opening {0}",fileName );

                throw e;
            }
        }

        private static FractalNode ParseYamlFile(StreamReader input)
        {
            var node = new FractalNode();
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            try
            {
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return node;
        }
    }
}