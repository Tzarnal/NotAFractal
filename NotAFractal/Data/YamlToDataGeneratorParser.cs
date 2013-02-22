using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using NotAFractal.Models;
using YamlDotNet.RepresentationModel;

namespace NotAFractal.Data
{
    public class YamlToDataGeneratorParser
    {
        public static Dictionary<string, DataGenerator> ParseGenerators()
        {
            var path = HostingEnvironment.MapPath(@"~/Data/DataGenerators");
            var generators = new Dictionary<string, DataGenerator>();

            if(path == null)
                throw new FileNotFoundException();

            var generatorFiles = Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories);

            foreach (var generatorFile in generatorFiles)
            {
                var generator = ReadFile(generatorFile);

                if( generator != null)
                {
                    // ReSharper disable AssignNullToNotNullAttribute
                    generators.Add(Path.GetFileNameWithoutExtension(generatorFile), generator);
                    // ReSharper restore AssignNullToNotNullAttribute
                }
            }

            return generators;
        }

        private static DataGenerator ReadFile( string fileName)
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
                Debug.WriteLine("Error Opening: " + fileName);

                throw;
            }            
        }

        private static DataGenerator ParseYamlFile(StreamReader input)
        {
            var generator = new DataGenerator {DataGeneratorEntries = new List<DataGeneratorEntry>()};
            var yaml = new YamlStream();

            try
            {
                yaml.Load(input);
                var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;
                
                foreach (YamlSequenceNode node in mapping)
                {
                    var generatorEntry = new DataGeneratorEntry {WeightedStrings = new Dictionary<string, int>()};

                    foreach (YamlSequenceNode subnode in node)
                    {
                        var subnodeData = subnode.Children;
                        
                        var text = subnodeData[0].ToString();
                        var weight = int.Parse(subnodeData[1].ToString());

                        generatorEntry.TotalWeight += weight;
                        generatorEntry.WeightedStrings.Add(text,weight);
                    }

                    generator.DataGeneratorEntries.Add(generatorEntry);
                }
            }            
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            return generator;
        }
    }
}