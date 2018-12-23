using System;
using System.Collections.Generic;
using System.Linq;
using TheTreasuresMap.Models;
using static TheTreasuresMap.Models.TreasureMap;
using static TheTreasuresMap.Mouvement;

namespace TheTreasuresMap
{
    public static class Tools
    {
        public static int MyConvertInt(string s)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(s);
            }
            catch (FormatException)
            {
                Console.WriteLine("The format is incorrect");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow");
            }
            return i;
        }

        public static List<Direction> MyConvertSequence(string s)
        {
            var dicDirection = new Dictionary<char, Direction>
            {
                { 'A', Direction.Forward },
                { 'G', Direction.Left },
                { 'D', Direction.Right },
            };

            var sequence = new List<Direction>();
            var c = new List<char>();
            c.AddRange(s);

            foreach (var a in c)
            {
                sequence.Add(dicDirection[a]);
            }
            return sequence;
        }

        public static void MyError(string msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }

        public static void DisplayMap(BoxeType[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    var val = map[x, y];
                    switch (val)
                    {
                        case BoxeType.Prairie:
                            Console.Write("P\t");
                            break;
                        case BoxeType.Mountain:
                            Console.Write("M\t");
                            break;
                        case BoxeType.Treasure:
                            Console.Write("T\t");
                            break;
                        case BoxeType.Adventurer:
                            Console.Write("A\t");
                            break;
                        case BoxeType.AdventurerTreasure:
                            Console.Write("AT\t");
                            break;
                    }
                }
                Console.Write(Environment.NewLine);
            }
        }

        public static void WriteOutFile(TreasureMap treasuresMap)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Constants.OutputPathFile))
            {
                var line = string.Empty;

                for (int y = 0; y < treasuresMap.Map.GetLength(0); y++)
                {
                    for (int x = 0; x < treasuresMap.Map.GetLength(1); x++)
                    {
                        var val = treasuresMap.Map[y, x];
                        switch (val)
                        {
                            case BoxeType.Prairie:
                                line = line += ".\t\t\t";
                                break;
                            case BoxeType.Mountain:
                                line = line += "M\t\t\t";
                                break;
                            case BoxeType.Treasure:
                                line = line += $"T({treasuresMap.Treasures.Single(t => t.Y == y && t.X == x).Nb})\t\t";
                                break;
                            case BoxeType.Adventurer:
                                line = line += $"A({treasuresMap.Adventurers.Single(t => t.Y == y && t.X == x).Name})\t\t\t";
                                break;
                            case BoxeType.AdventurerTreasure:
                                line = line += $"A({treasuresMap.Adventurers.Single(t => t.Y == y && t.X == x).Name})T({treasuresMap.Treasures.Single(t => t.Y == y && t.X == x).Nb})\t";
                                break;
                        }
                    }
                    line = line += Environment.NewLine;
                }
                file.WriteLine(line);
            }
        }
    }
}
