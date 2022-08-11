using LowRezJam22.Engine.Graphics;
using LowRezJam22.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22
{
    internal struct LevelDefinition
    {
        public int Background = 0;
        public string NextLevel = "";
        public string Map = "";
        //x - smarttile
        //y - blocktile
        //u - changeup
        //d - changedown
        //w - water
        //f - flag
        //a - enemy 1 static down
        //A - enemy 2 static down
        //b - enemy 1 static up
        //B - enemy 2 static up
        //c - enemy 1 moving down
        //C - enemy 2 moving down
        //e - enemy 1 moving up
        //E - enemy 2 moving up
        //p - playerstart down
        //P - playerstart up
        public TileDefinition mainTile;
        public Texture enemyTexture1;
        public Texture enemyTexture2;

        public LevelDefinition(int background, string nextLevel, string map, TileDefinition mainTile, Texture enemyTexture1, Texture enemyTexture2)
        {
            Background = background;
            NextLevel = nextLevel;
            Map = map;
            this.mainTile = mainTile;
            this.enemyTexture1 = enemyTexture1;
            this.enemyTexture2 = enemyTexture2;
        }
    }
}
