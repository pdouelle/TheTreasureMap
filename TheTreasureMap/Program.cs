using System;
using TheTreasuresMap.Models;

namespace TheTreasuresMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var project = new Project();
            var treasureMap = new TreasureMap();
            project.Execute(treasureMap);
            Console.ReadLine();
        }
    }
}
