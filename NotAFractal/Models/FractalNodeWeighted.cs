using System;

namespace NotAFractal.Models
{
    public class FractalNodeWeighted
    {
        public int PercentageChance { get; set; } //Percentage chance this node will actually be added to the list
        public int MinAmount { get; set; } //If this weighted element gets added, how many are added minimum
        public int MaxAmount { get; set; } //If this weighted element gets added, how many are added maximum

        public String Type { get; set; } //What it actually is
    }
}