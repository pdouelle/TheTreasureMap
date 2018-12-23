using System;
using System.Collections.Generic;
using System.Text;

namespace TheTreasuresMap.Models
{
    public class TreasureMap
    {
        public enum BoxeType { Prairie, Mountain, Treasure, Adventurer, AdventurerTreasure };

        public BoxeType[,] Map;
        public List<Tuple<int, int>> Mountains = new List<Tuple<int, int>>();
        public List<Adventurer> Adventurers = new List<Adventurer>();
        public List<Treasure> Treasures = new List<Treasure>();
    }
}
