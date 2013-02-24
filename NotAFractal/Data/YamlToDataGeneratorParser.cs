﻿using System;
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
        private static string _curFileName;
        
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
                return null;
            }            
        }

        private static DataGenerator ParseYamlFile(YamlStream yaml)
        {
            var generator = new DataGenerator {DataGeneratorEntries = new List<WeightedChoiceEntry>()};
            //See YamlToNodePArser.cs to get a better idea of how handle yamldontnet 

            try
            {
                var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;
                
                foreach (YamlSequenceNode node in mapping)
                {
                    var generatorEntry = new WeightedChoiceEntry {WeightedStrings = new Dictionary<string, int>()};

                    foreach (YamlSequenceNode subnode in node)
                    {
                        var subnodeData = subnode.Children;
                        
                        var text = subnodeData[0].ToString();
                        
                        //Parsing into an int can be tricky and users are likely to put in something that won't parse so we need more sanity checks here
                        var weight = 1;
                        if(!int.TryParse( subnodeData[1].ToString(), out weight))
                        {
                            Debug.WriteLine("Could not parse int from subnode: {0} defaulting to 1", subnode);
                        }

                        generatorEntry.TotalWeight += weight;
                        generatorEntry.WeightedStrings.Add(text,weight);
                    }

                    generator.DataGeneratorEntries.Add(generatorEntry);
                }
            }            
            catch (Exception e)
            {
                Debug.WriteLine("Error parsing: {0}", _curFileName);
                Debug.WriteLine(e);
                return null;
            }

            return generator;
        }
    }
}