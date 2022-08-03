using LowRezJam22.Engine.Graphics;
using LowRezJam22.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.SmartTiles
{
    internal class DesertTile : Engine.Tiles.SmartTile
    {
        public List<Texture> LeftUp { get; }
        public List<Texture> LeftCenter { get; }
        public List<Texture> LeftDown { get; }
        public List<Texture> MiddleUp { get; }
        public List<Texture> MiddleCenter { get; }
        public List<Texture> MiddleDown { get; }
        public List<Texture> RightUp { get; }
        public List<Texture> RightCenter { get; }
        public List<Texture> RightDown { get; }
        public List<Texture> VerticalTop{ get; }
        public List<Texture> VerticalCenter { get; }
        public List<Texture> VerticalBottom { get; }
        public List<Texture> HorionzalLeft { get; }
        public List<Texture> HorionzalCenter { get; }
        public List<Texture> HorionzalRight { get; }
        public List<Texture> Filled { get; }

        public DesertTile(List<Texture> leftUp, List<Texture> leftCenter, List<Texture> leftDown,
                          List<Texture> middleUp, List<Texture> middleCenter, List<Texture> middleDown,
                          List<Texture> rightUp, List<Texture> rightCenter, List<Texture> rightDown,
                          List<Texture> verticalTop, List<Texture> verticalCenter, List<Texture> verticalBottom,
                          List<Texture> horizontalLeft, List<Texture> horionzalCenter, List<Texture> horizontalRight,
                          List<Texture> filled)
        {
            LeftUp = leftUp;
            LeftCenter = leftCenter;
            LeftDown = leftDown;
            MiddleUp = middleUp;
            MiddleCenter = middleCenter;
            MiddleDown = middleDown;
            RightUp = rightUp;
            RightCenter = rightCenter;
            RightDown = rightDown;
            VerticalTop = verticalTop;
            VerticalCenter = verticalCenter;
            VerticalBottom = verticalBottom;
            HorionzalLeft = horizontalLeft;
            HorionzalCenter = horionzalCenter;
            HorionzalRight = horizontalRight;
            Filled = filled;
        }

        public override Texture? GetTexture(int X, int Y, TileMap map)
        {
            int flags = BuildFlags(X, Y, map);
            return flags switch
            {
                1 => GetRandomTexture(VerticalBottom),
                2 => GetRandomTexture(HorionzalLeft),
                3 => GetRandomTexture(LeftDown),
                4 => GetRandomTexture(VerticalTop),
                5 => GetRandomTexture(VerticalCenter),
                6 => GetRandomTexture(LeftUp),
                7 => GetRandomTexture(LeftCenter),
                8 => GetRandomTexture(HorionzalRight),
                9 => GetRandomTexture(RightDown),
                10 => GetRandomTexture(HorionzalCenter),
                11 => GetRandomTexture(MiddleDown),
                12 => GetRandomTexture(RightUp),
                13 => GetRandomTexture(RightCenter),
                14 => GetRandomTexture(MiddleUp),
                15 => GetRandomTexture(MiddleCenter),
                _ => GetRandomTexture(Filled),
            };
        }

        private int BuildFlags(int X, int Y, TileMap map)
        {
            int flags = 0;
            if (CheckIfTile(X, Y - 1, map)) flags += 1;
            if (CheckIfTile(X + 1, Y, map)) flags += 2;
            if (CheckIfTile(X, Y + 1, map)) flags += 4;
            if (CheckIfTile(X - 1, Y, map)) flags += 8;
            return flags;
        }

        private bool CheckIfTile(int X, int Y, TileMap map)
        {
            TileDefinition? tile = map.GetTileAt(X, Y);
            if (tile is null)
                return false;
            if (tile.GetType() != typeof(DesertTile))
                return false;
            return true;
        }

        private Texture GetRandomTexture(List<Texture> list)
        {
            return list[Random.Shared.Next(0, list.Count)];
        }
    }
}
