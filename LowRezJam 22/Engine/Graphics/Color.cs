using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.Engine.Graphics
{
    internal class Color
    {
        public float R { get; set; } = 0;
        public float G { get; set; } = 0;
        public float B { get; set; } = 0;
        public float A { get; set; } = 0;

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int r, int g, int b, int a)
        {
            R = r / 255f;
            G = g / 255f;
            B = b / 255f;
            A = a / 255f;
        }

        public static Color FromHSLA(float H, float S, float L, float A)
        {
            H %= 360;
            float C = (1 - Math.Abs(2 * L - 1)) * S;
            float X = C * (1 - Math.Abs(H / 60 % 2 - 1)); //Maybe radians?
            float m = L - C / 2;

            float r = 0;
            float g = 0;
            float b = 0;
            if (H >= 0 && H < 60)
            {
                r = C;
                g = X;
                b = 0;
            }
            else if (H >= 60 && H < 120)
            {
                r = X;
                g = C;
                b = 0;
            }
            else if (H >= 120 && H < 180)
            {
                r = 0;
                g = C;
                b = X;
            }
            else if (H >= 180 && H < 240)
            {
                r = 0;
                g = X;
                b = C;
            }
            else if (H >= 240 && H < 300)
            {
                r = X;
                g = 0;
                b = C;
            }
            else if (H >= 300 && H < 360)
            {
                r = C;
                g = 0;
                b = X;
            }
            return new Color(r + m, g + m, b + m, A);
        }

        public (float H, float S, float L, float A) GetHSLA()
        {
            float CMax = R;
            if (G > CMax) CMax = G;
            if (B > CMax) CMax = B;

            float CMin = R;
            if (G < CMin) CMin = G;
            if (B < CMin) CMin = B;

            float delta = CMax - CMin;

            float H = 0;
            if (CMax == R)
            {
                H = 60 * ((G - B) / delta % 6);
            }
            else if (CMax == G)
            {
                H = 60 * ((B - R) / delta + 2);
            }
            else if (CMax == B)
            {
                H = 60 * ((R - G) / delta + 4);
            }
            float L = (CMax + CMin) / 2;
            float S;
            if (delta == 0)
            {
                S = 0;
            }
            else
            {
                S = delta / (1 - Math.Abs(2 * L - 1));
            }
            return (H, S, L, A);
        }

        public static implicit operator OpenTK.Mathematics.Color4(Color source)
        {
            return new OpenTK.Mathematics.Color4(source.R, source.G, source.B, source.A);
        }

        public static implicit operator Color(OpenTK.Mathematics.Color4 source)
        {
            return new Color(source.R, source.G, source.B, source.A);
        }

        public static implicit operator OpenTK.Mathematics.Vector4(Color source)
        {
            return new OpenTK.Mathematics.Vector4(source.R, source.G, source.B, source.A);
        }

        public static implicit operator Color(OpenTK.Mathematics.Vector4 source)
        {
            return new Color(source.X, source.Y, source.Z, source.W);
        }
    }
}
