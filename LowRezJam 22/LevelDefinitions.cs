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
        public static Dictionary<string, LevelDefinition> Definitions = new();

        private static SmartTiles.DesertTile sandTile;
        private static SmartTiles.DesertTile pyramidTile;
        private static SmartTiles.DesertTile caveTile;
        private static SmartTiles.DesertTile marsTile;

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

            sandTile = new(new() { desertTiles[0] }, new() { desertTiles[1], desertTiles[2] }, new() { desertTiles[3] },
                new() { desertTiles[4], desertTiles[5] }, new() { desertTiles[6], desertTiles[7] }, new() { desertTiles[8], desertTiles[9] },
                new() { desertTiles[10] }, new() { desertTiles[11], desertTiles[12] }, new() { desertTiles[13] },
                new() { desertTiles[14] }, new() { desertTiles[15] }, new() { desertTiles[16] },
                new() { desertTiles[17] }, new() { desertTiles[18] }, new() { desertTiles[19] },
                new() { desertTiles[20] });

            pyramidTile = new(new() { pyramidTiles[0] }, new() { pyramidTiles[1], pyramidTiles[2] }, new() { pyramidTiles[3] },
                new() { pyramidTiles[4], pyramidTiles[5] }, new() { pyramidTiles[6], pyramidTiles[7] }, new() { pyramidTiles[8], pyramidTiles[9] },
                new() { pyramidTiles[10] }, new() { pyramidTiles[11], pyramidTiles[12] }, new() { pyramidTiles[13] },
                new() { pyramidTiles[14] }, new() { pyramidTiles[15] }, new() { pyramidTiles[16] },
                new() { pyramidTiles[17] }, new() { pyramidTiles[18] }, new() { pyramidTiles[19] },
                new() { pyramidTiles[20] });

            caveTile = new(new() { caveTiles[0] }, new() { caveTiles[1], caveTiles[2] }, new() { caveTiles[3] },
                new() { caveTiles[4], caveTiles[5] }, new() { caveTiles[6], caveTiles[7] }, new() { caveTiles[8], caveTiles[9] },
                new() { caveTiles[10] }, new() { caveTiles[11], caveTiles[12] }, new() { caveTiles[13] },
                new() { caveTiles[14] }, new() { caveTiles[15] }, new() { caveTiles[16] },
                new() { caveTiles[17] }, new() { caveTiles[18] }, new() { caveTiles[19] },
                new() { caveTiles[20] });

            marsTile = new(new() { marsTiles[0] }, new() { marsTiles[1], marsTiles[2] }, new() { marsTiles[3] },
                new() { marsTiles[4], marsTiles[5] }, new() { marsTiles[6], marsTiles[7] }, new() { marsTiles[8], marsTiles[9] },
                new() { marsTiles[10] }, new() { marsTiles[11], marsTiles[12] }, new() { marsTiles[13] },
                new() { marsTiles[14] }, new() { marsTiles[15] }, new() { marsTiles[16] },
                new() { marsTiles[17] }, new() { marsTiles[18] }, new() { marsTiles[19] },
                new() { marsTiles[20] });

            //Order: 3, 2, 0, 1, 4, 5, 6, 7, 8
            AddDefinition(3, "Level0", "Level1");
            AddDefinition(2, "Level1", "Level2");
            AddDefinition(0, "Level2", "Level3");
            AddDefinition(1, "Level3", "Level4");
            AddDefinition(4, "Level4", "Level5");
            AddDefinition(5, "Level5", "Level6");
            AddDefinition(6, "Level6", "Level7");
            AddDefinition(7, "Level7", "Level8");
            AddDefinition(8, "Level8", "Level9");
        }

        public static void AddDefinition(int type, string name, string nextName)
        {
            if (!File.Exists("LevelMaps/" + name + ".txt"))
            {
                return;
            }
            string levelMap = File.ReadAllText("LevelMaps/" + name + ".txt");
            Engine.Tiles.TileDefinition mainTile;
            string enemy1;
            string enemy2;
            switch (type)
            {
                case 0:
                    mainTile = sandTile;
                    enemy1 = "Assets/Enemies/Enemies_0.png";
                    enemy2 = "Assets/Enemies/Enemies_1.png";
                    break;
                case 1:
                    mainTile = sandTile;
                    enemy1 = "Assets/Enemies/Enemies_0.png";
                    enemy2 = "Assets/Enemies/Enemies_1.png";
                    break;
                case 2:
                    mainTile = sandTile;
                    enemy1 = "Assets/Enemies/Enemies_0.png";
                    enemy2 = "Assets/Enemies/Enemies_1.png";
                    break;
                case 3:
                    mainTile = sandTile;
                    enemy1 = "Assets/Enemies/Enemies_0.png";
                    enemy2 = "Assets/Enemies/Enemies_1.png";
                    break;
                case 4:
                    mainTile = pyramidTile;
                    enemy1 = "Assets/Enemies/Enemies_2.png";
                    enemy2 = "Assets/Enemies/Enemies_3.png";
                    break;
                case 5:
                    mainTile = pyramidTile;
                    enemy1 = "Assets/Enemies/Enemies_2.png";
                    enemy2 = "Assets/Enemies/Enemies_3.png";
                    break;
                case 6:
                    mainTile = caveTile;
                    enemy1 = "Assets/Enemies/Enemies_4.png";
                    enemy2 = "Assets/Enemies/Enemies_5.png";
                    break;
                case 7:
                    mainTile = caveTile;
                    enemy1 = "Assets/Enemies/Enemies_4.png";
                    enemy2 = "Assets/Enemies/Enemies_5.png";
                    break;
                case 8:
                default:
                    mainTile = marsTile;
                    enemy1 = "Assets/Enemies/Enemies_6.png";
                    enemy2 = "Assets/Enemies/Enemies_7.png";
                    break;
            }
            Definitions.Add(name, new(type, nextName, levelMap, mainTile, new(enemy1), new(enemy2)));
        }
    }
}
