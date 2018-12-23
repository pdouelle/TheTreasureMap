using System;
using System.Collections.Generic;
using System.Linq;
using TheTreasuresMap.Models;
using static TheTreasuresMap.Models.TreasureMap;

namespace TheTreasuresMap
{
    public class Mouvement
    {
        public enum Cardinal { North, East, South, West };
        public enum Direction { Forward, Left, Right }

        public static Dictionary<string, Cardinal> DicCardinal = new Dictionary<string, Cardinal>()
        {
            { "N", Cardinal.North },
            { "E", Cardinal.East },
            { "S", Cardinal.South },
            { "W", Cardinal.West }
        };

        /// <summary>
        /// Advance the adventurer
        /// </summary>
        /// <param name="treasureMap"></param>
        /// <param name="adventurer"></param>
        /// <param name="destY"></param>
        /// <param name="destX"></param>
        /// <returns></returns>
        public static TreasureMap Forward(TreasureMap treasureMap, Adventurer adventurer, int destY, int destX)
        {
            if (treasureMap.Map[destY, destX] != BoxeType.Adventurer && treasureMap.Map[destY, destX] != BoxeType.Mountain && treasureMap.Map[destY, destX] != BoxeType.AdventurerTreasure)
            {
                if (treasureMap.Map[adventurer.Y, adventurer.X] == BoxeType.AdventurerTreasure)
                    treasureMap.Map[adventurer.Y, adventurer.X] = BoxeType.Treasure;
                else
                    treasureMap.Map[adventurer.Y, adventurer.X] = BoxeType.Prairie;

                if (treasureMap.Map[destY, destX] == BoxeType.Treasure)
                {
                    var treasure = treasureMap.Treasures.Single(w => w.Y == destY && w.X == destX);
                    if (treasure.Nb > 0)
                    {
                        treasure.Nb--;
                        adventurer.NbTreasure++;
                        treasureMap.Map[destY, destX] = BoxeType.AdventurerTreasure;
                    }
                }
                else
                    treasureMap.Map[destY, destX] = BoxeType.Adventurer;
                adventurer.Y = destY;
                adventurer.X = destX;
            }
            return treasureMap;
        }

        /// <summary>
        /// Change the orientation of the adventurer.
        /// </summary>
        /// <param name="adventurer"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Adventurer ChangeDirection(Adventurer adventurer, Direction direction)
        {
            if (direction == Direction.Left && adventurer.Direction == Cardinal.North)
                adventurer.Direction = Cardinal.West;
            else if (direction == Direction.Right && adventurer.Direction == Cardinal.West)
                adventurer.Direction = Cardinal.North;
            else if (direction == Direction.Left)
                adventurer.Direction--;
            else if (direction == Direction.Right)
                adventurer.Direction++;
            return adventurer;
        }
    }
}