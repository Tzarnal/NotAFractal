using System;
using System.Collections.Generic;

namespace NotAFractal.Models
{
    public class WeightedChoiceEntry
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