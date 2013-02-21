using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NotAFractal.Models
{
    public class DataGenerator
    {
        public List<DataGeneratorEntry> DataGeneratorEntries { get; set; }

        public string Generate(int seed)
        {
            var result = new StringBuilder();
            var random = new Random(seed);

            foreach (var dataGeneratorEntry in DataGeneratorEntries)
            {
                result.Append(dataGeneratorEntry.Generate(random.Next(1, int.MaxValue)));
            }

            return result.ToString();
        }
    }
}