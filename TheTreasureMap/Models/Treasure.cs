using System.Text.RegularExpressions;

namespace TheTreasuresMap.Models
{
    public class Treasure
    {
        public Treasure(Match match)
        {
            X = Tools.MyConvertInt(match.Groups["x"].Value);
            Y = Tools.MyConvertInt(match.Groups["y"].Value);
            Nb = Tools.MyConvertInt(match.Groups["nb"].Value);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Nb { get; set; }
    }
}
