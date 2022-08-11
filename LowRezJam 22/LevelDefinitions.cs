using LowRezJam22.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22
{
    internal static class LevelDefinitions
    {
        public static Dictionary<string, LevelDefinition> Defintions = new();

        public static void LoadDefs()
        {
            List<Texture> desertTiles = new();
            for (int i = 0; i <= 20; i++)
            {
                desertTiles.Add(new Texture("Assets/Tiles/DesertTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            SmartTiles.DesertTile sandTile = new(new() { desertTiles[0] }, new() { desertTiles[1], desertTiles[2] }, new() { desertTiles[3] },
                new() { desertTiles[4], desertTiles[5] }, new() { desertTiles[6], desertTiles[7] }, new() { desertTiles[8], desertTiles[9] },
                new() { desertTiles[10] }, new() { desertTiles[11], desertTiles[12] }, new() { desertTiles[13] },
                new() { desertTiles[14] }, new() { desertTiles[15] }, new() { desertTiles[16] },
                new() { desertTiles[17] }, new() { desertTiles[18] }, new() { desertTiles[19] },
                new() { desertTiles[20] });
            string l0map =
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxx-----e---xxxxx-------------------ddd------xxxx\n" +
            "xxxx--------y-B---y---------------------------xxxx\n" +
            "xxxx------------------------------------------xxxx\n" +
            "xxxx------------------------------------------xxxx\n" +
            "xxx--------------------------------------------xxx\n" +
            "------------w-------------------------------------\n" +
            "--------------------------------------------------\n" +
            "xxx-------------------------------w------------xxx\n" +
            "xxx-------y-C-uu--y----------------------------xxx\n" +
            "xxx-p---a--xxxxxxx-------------------------f---xxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n";
            LevelDefinition d = new(0, "Level0", l0map, sandTile, 
                new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level0", d);
        }
    }
}
