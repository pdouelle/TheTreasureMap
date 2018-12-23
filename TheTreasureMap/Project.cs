using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TheTreasuresMap.Models;
using static TheTreasuresMap.Models.TreasureMap;
using static TheTreasuresMap.Mouvement;

namespace TheTreasuresMap
{
    public class Project
    {
        private delegate TreasureMap MoveDelagate(TreasureMap treasureMap, Adventurer adventurer);

        private readonly Dictionary<Cardinal, MoveDelagate> DicForward;

        public Project()
        {
            DicForward = new Dictionary<Cardinal, MoveDelagate>()
            {
                { Cardinal.North, (treasureMap, adventurer) => Forward(treasureMap, adventurer, adventurer.Y - 1, adventurer.X)},
                { Cardinal.East, (treasureMap, adventurer) => Forward(treasureMap, adventurer, adventurer.Y, adventurer.X + 1)},
                { Cardinal.South, (treasureMap, adventurer) => Forward(treasureMap, adventurer, adventurer.Y + 1, adventurer.X)},
                { Cardinal.West, (treasureMap, adventurer) => Forward(treasureMap, adventurer, adventurer.Y, adventurer.X - 1)}
            };
        }

        public TreasureMap Execute(TreasureMap treasureMap)
        {
            treasureMap = Parsing(treasureMap);
            treasureMap = CreateMap(treasureMap);
            treasureMap = Scenario(treasureMap);
            Tools.WriteOutFile(treasureMap);
            return treasureMap;
        }

        /// <summary>
        /// Parse the file
        /// </summary>
        /// <param name="treasureMap"></param>
        /// <returns></returns>
        public TreasureMap Parsing(TreasureMap treasureMap)
        {
            Regex regex = new Regex(@"^(?<code>A|C|M|T)(?: - (?<name>\D+))? - (?<x>\d+) - (?<y>\d+)(?: - (?<direction>[N|S|O|E]) - (?<sequence>[A|G|D]+)| - (?<nb>\d))?");

            using (StreamReader reader = new StreamReader(Constants.InputPathFile))
            {
                string line;
                int nbLineMapFound = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                        treasureMap = AddByCode(treasureMap, match, ref nbLineMapFound);
                    else
                        Console.WriteLine($"Warn: This line has been ignored. : {line}");
                }
            }
            return treasureMap;
        }

        /// <summary>
        /// Fills the objects according to the elements of the file.
        /// </summary>
        /// <param name="treasureMap"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public TreasureMap AddByCode(TreasureMap treasureMap, Match match, ref int nbLineMapFound)
        {
            switch (match.Groups["code"].Value)
            {
                case "C":
                    treasureMap.Map = new BoxeType[Tools.MyConvertInt(match.Groups["y"].Value), Tools.MyConvertInt(match.Groups["x"].Value)];
                    nbLineMapFound++;
                    break;
                case "M":
                    treasureMap.Mountains.Add(new Tuple<int, int>(Tools.MyConvertInt(match.Groups["y"].Value), Tools.MyConvertInt(match.Groups["x"].Value)));
                    break;
                case "T":
                    treasureMap.Treasures.Add(new Treasure(match));
                    break;
                case "A":
                    treasureMap.Adventurers.Add(new Adventurer(match));
                    break;
            }

            if (nbLineMapFound > 1)
                Tools.MyError("Error : Several map lines are present in the file");
            return treasureMap;
        }

        /// <summary>
        /// Create the map
        /// </summary>
        /// <param name="treasureMap"></param>
        /// <returns></returns>
        public TreasureMap CreateMap(TreasureMap treasureMap)
        {
            try
            {
                foreach (var mountain in treasureMap.Mountains)
                {
                    if (treasureMap.Map[mountain.Item1, mountain.Item2] != BoxeType.Prairie)
                        Tools.MyError($"You want to insert a {BoxeType.Mountain} but an object is already present there y:{mountain.Item1} x:{mountain.Item2} objet:{treasureMap.Map[mountain.Item1, mountain.Item2]}");
                    treasureMap.Map[mountain.Item1, mountain.Item2] = BoxeType.Mountain;
                }
                foreach (var treasure in treasureMap.Treasures)
                {
                    if (treasureMap.Map[treasure.Y, treasure.X] != BoxeType.Prairie)
                        Tools.MyError($"You want to insert a {BoxeType.Treasure} but an object is already present there y:{treasure.Y} x:{treasure.X} objet:{treasureMap.Map[treasure.Y, treasure.X]}");
                    treasureMap.Map[treasure.Y, treasure.X] = BoxeType.Treasure;
                }
                foreach (var adventurer in treasureMap.Adventurers)
                {
                    if (treasureMap.Map[adventurer.Y, adventurer.X] != BoxeType.Prairie)
                        Tools.MyError($"You want to insert a {BoxeType.Adventurer} but an object is already present there y:{adventurer.Y} x:{adventurer.X} objet:{treasureMap.Map[adventurer.Y, adventurer.X]}");
                    treasureMap.Map[adventurer.Y, adventurer.X] = BoxeType.Adventurer;
                }
            }
            catch (IndexOutOfRangeException oe)
            {
                Tools.MyError($"The place you want to access is outside the map. : {oe.Message}");
            }
            return treasureMap;
        }

        /// <summary>
        /// Execute the sequence of adventurers.
        /// </summary>
        /// <param name="treasureMap"></param>
        /// <returns></returns>
        public TreasureMap Scenario(TreasureMap treasureMap)
        {
            int i = 0;
            var adventurers = treasureMap.Adventurers;
            var mouvement = new Mouvement();

            while (i < treasureMap.Adventurers.Select(m => m.Sequence).Max(c => c.Count))
            {
                int j = 0;
                while (j < adventurers.Count())
                {
                    if (i < adventurers[j].Sequence.Count)
                    {
                        switch (adventurers[j].Sequence[i])
                        {
                            case Direction.Forward:
                                treasureMap = DicForward[adventurers[j].Direction](treasureMap, adventurers[j]);
                                break;
                            case Direction.Left:
                                adventurers[j] = mouvement.ChangeDirection(adventurers[j], Direction.Left);
                                break;
                            case Direction.Right:
                                adventurers[j] = mouvement.ChangeDirection(adventurers[j], Direction.Right);
                                break;
                        };
                    }
                    j++;
                }
                i++;
            }
            treasureMap.Adventurers = adventurers;
            return treasureMap;
        }
    }
}
