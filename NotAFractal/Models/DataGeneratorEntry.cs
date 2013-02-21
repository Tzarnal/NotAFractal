using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotAFractal.Models
{
    public class DataGeneratorEntry
    {
        public Dictionary<string, int> WeightedStrings { get; set; }
        public int TotalWeight { get; set; }

        public string Generate(int seed)
        {
            var random = new Random(seed);
            var r = random.Next(0, TotalWeight);

            string selectedString = "";

            foreach (var weightedString in WeightedStrings)
            {
                if( r < weightedString.Value)
                {
                    selectedString = weightedString.Key;
                    break;
                }

                r -= weightedString.Value;
            }

            return selectedString;
        }
    }
}