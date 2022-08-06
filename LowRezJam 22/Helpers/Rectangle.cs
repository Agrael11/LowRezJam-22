using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.Helpers
{
    public class Rectangle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float X2 { get { return X + Width; } }
        public float Y2 { get { return Y + Height; } }
        public Vector2 Location { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }
        public Vector2 Size { get { return new Vector2(Width, Height); } set { Width = value.X; Height = value.Y; } }

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            if (Width < 0)
            {
                X += Width;
                Width *= -1;
            }
            Height = height;
            if (Height < 0)
            {
                Y += Height;
                Height *= -1;
            }
        }

        public Rectangle(Vector2 location, float width, float height) : this(location.X, location.Y, width, height)
        {
        }

        public Rectangle(float x, float y, Vector2 size) : this(x, y, size.X, size.Y)
        {
        }

        public Rectangle(Vector2 location, Vector2 size) : this(location.X, location.Y, size.X, size.Y)
        {
        }

        public static bool Intersects(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (rectangle1.X < rectangle2.X2 && rectangle1.X2 > rectangle2.X && rectangle1.Y < rectangle2.Y2 && rectangle1.Y2 > rectangle2.Y);
        }

        public  bool Intersects(Rectangle rectangle)
        {
            return (X < rectangle.X2 && X2 > rectangle.X && Y < rectangle.Y2 && Y2 > rectangle.Y);
        }

        public override string ToString()
        {
            return $"{X}x{Y} {Width}x{Height}";
        }
    }
}
