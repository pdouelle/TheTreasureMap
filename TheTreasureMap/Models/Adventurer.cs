using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static TheTreasuresMap.Mouvement;
using static TheTreasuresMap.Project;

namespace TheTreasuresMap.Models
{
    public class Adventurer
    {
        public Adventurer(Match match)
        {
            Name = match.Groups["name"].Value;
            X = Tools.MyConvertInt(match.Groups["x"].Value);
            Y = Tools.MyConvertInt(match.Groups["y"].Value);
            Direction = DicCardinal[match.Groups["direction"].Value];
            Sequence = Tools.MyConvertSequence(match.Groups["sequence"].Value);
        }

        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Cardinal Direction { get; set; }
        public List<Direction> Sequence { get; set; }
        public int NbTreasure { get; set; } = 0;
    }
}
