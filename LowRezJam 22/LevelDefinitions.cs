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
                desertTiles.Add(new Texture("Assets/Tiles/Desert/DesertTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }
            List<Texture> pyramidTiles = new();
            for (int i = 0; i <= 20; i++)
            {
                pyramidTiles.Add(new Texture("Assets/Tiles/Pyramid/PyramidTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            List<Texture> caveTiles = new();
            for (int i = 0; i <= 20; i++)
            {
                caveTiles.Add(new Texture("Assets/Tiles/Cave/CaveTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            List<Texture> marsTiles = new();
            for (int i = 0; i <= 20; i++)
            {
                marsTiles.Add(new Texture("Assets/Tiles/Mars/MarsTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            SmartTiles.DesertTile sandTile = new(new() { desertTiles[0] }, new() { desertTiles[1], desertTiles[2] }, new() { desertTiles[3] },
                new() { desertTiles[4], desertTiles[5] }, new() { desertTiles[6], desertTiles[7] }, new() { desertTiles[8], desertTiles[9] },
                new() { desertTiles[10] }, new() { desertTiles[11], desertTiles[12] }, new() { desertTiles[13] },
                new() { desertTiles[14] }, new() { desertTiles[15] }, new() { desertTiles[16] },
                new() { desertTiles[17] }, new() { desertTiles[18] }, new() { desertTiles[19] },
                new() { desertTiles[20] });

            SmartTiles.DesertTile pyramidTile = new(new() { pyramidTiles[0] }, new() { pyramidTiles[1], pyramidTiles[2] }, new() { pyramidTiles[3] },
                new() { pyramidTiles[4], pyramidTiles[5] }, new() { pyramidTiles[6], pyramidTiles[7] }, new() { pyramidTiles[8], pyramidTiles[9] },
                new() { pyramidTiles[10] }, new() { pyramidTiles[11], pyramidTiles[12] }, new() { pyramidTiles[13] },
                new() { pyramidTiles[14] }, new() { pyramidTiles[15] }, new() { pyramidTiles[16] },
                new() { pyramidTiles[17] }, new() { pyramidTiles[18] }, new() { pyramidTiles[19] },
                new() { pyramidTiles[20] });

            SmartTiles.DesertTile caveTile = new(new() { caveTiles[0] }, new() { caveTiles[1], caveTiles[2] }, new() { caveTiles[3] },
                new() { caveTiles[4], caveTiles[5] }, new() { caveTiles[6], caveTiles[7] }, new() { caveTiles[8], caveTiles[9] },
                new() { caveTiles[10] }, new() { caveTiles[11], caveTiles[12] }, new() { caveTiles[13] },
                new() { caveTiles[14] }, new() { caveTiles[15] }, new() { caveTiles[16] },
                new() { caveTiles[17] }, new() { caveTiles[18] }, new() { caveTiles[19] },
                new() { caveTiles[20] });

            SmartTiles.DesertTile marsTile = new(new() { marsTiles[0] }, new() { marsTiles[1], marsTiles[2] }, new() { marsTiles[3] },
                new() { marsTiles[4], marsTiles[5] }, new() { marsTiles[6], marsTiles[7] }, new() { marsTiles[8], marsTiles[9] },
                new() { marsTiles[10] }, new() { marsTiles[11], marsTiles[12] }, new() { marsTiles[13] },
                new() { marsTiles[14] }, new() { marsTiles[15] }, new() { marsTiles[16] },
                new() { marsTiles[17] }, new() { marsTiles[18] }, new() { marsTiles[19] },
                new() { marsTiles[20] });

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
            LevelDefinition d = new(0, "Level1", l0map, sandTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level0", d);
            d = new(1, "Level2", l0map, sandTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level1", d);
            d = new(2, "Level3", l0map, sandTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level2", d);
            d = new(3, "Level4", l0map, sandTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level3", d);
            d = new(4, "Level5", l0map, pyramidTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level4", d);
            d = new(5, "Level6", l0map, pyramidTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level5", d);
            d = new(6, "Level7", l0map, caveTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level6", d);
            d = new(7, "Level8", l0map, caveTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level7", d);
            /*LevelDefinition */d = new(8, "Level0", l0map, marsTile,
            new("Assets/Enemies/Cactus_0.png"), new("Assets/Enemies/Cactus_1.png"));
            Defintions.Add("Level8", d);
            //Defintions.Add("Level0", d);
        }
    }
}
