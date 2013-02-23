using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NotAFractal.Data;
using NotAFractal.Models.Builders;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models
{
    public class ModelManager
    {
        //YamlNodeManager is a singleton, to ensure the potentially large set of nodes only exists once.
        private static ModelManager _instance;
        
        private readonly Dictionary<string,FractalNode> _nodeList;
        private readonly NodeViewModelBuilder _nodeViewModelBuilder;
        private readonly NodeInformationViewModelBuilder _nodeInformationViewModelBuilder;
        private readonly Dictionary<string, DataGenerator> _dataGenerators;

        public static ModelManager Instance
        {
            get { return _instance ?? (_instance = new ModelManager()); }
        }
            
        private ModelManager()
        {
            _nodeList = YamlToNodeParser.ParseNodes();
            _nodeViewModelBuilder  = new NodeViewModelBuilder();
            _nodeInformationViewModelBuilder = new NodeInformationViewModelBuilder();
            _dataGenerators = YamlToDataGeneratorParser.ParseGenerators();
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

            return _nodeInformationViewModelBuilder.Build(seed, _nodeList[type]);
        }

        public string ProcessDataGeneratorSymbols(int seed, string text)
        {
            if (text == null)
                return null;

            var match = Regex.Match(text, @"\$[\w\d-]+\$");            
            var nestedDepth = 20;
            var random = new Random(seed);

            while (match.Success && nestedDepth > 0)
            {
                var symbols = match.Groups;
                
                foreach (var symbol in symbols)
                {
                    var symbolString = symbol.ToString();
                    var strippedSymbol = symbolString.Replace("$", string.Empty);

                    //Random number insertion
                    var randMatch = Regex.Match(strippedSymbol, @"Random-(\d+)-(\d+)");
                    if( randMatch.Success)
                    {
                        int min = int.Parse(randMatch.Groups[1].Value);
                        int max = int.Parse(randMatch.Groups[2].Value);

                        text = text.Replace(symbolString, random.Next(min,max).ToString());
                    }

                    if(_dataGenerators.ContainsKey(strippedSymbol))
                    {
                        var generator = _dataGenerators[strippedSymbol];
                        text = text.Replace(symbolString, generator.Generate(random.Next(1, int.MaxValue)));
                    }

                }

                nestedDepth--;
                match = Regex.Match(text, @"\$\w+\$");
            }

            return text;
        }
    }
}