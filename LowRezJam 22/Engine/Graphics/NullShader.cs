using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.Engine.Graphics
{
    internal class NullShader : Shader
    {
        public NullShader(string fragmentFilePath, string vertexFilePath) : base()
        {
        }

        public NullShader() : base()
        {

        }

        public new void UseShader()
        {
        }

        public new int GetAttributeLocation(string attribute)
        {
            return -1;
        }

        public new void SetInt(string attribute, int value)
        {
        }

        public new void SetFloat(string attribute, float value)
        {
        }

        public new void SetDouble(string attribute, double value)
        {
        }

        public new void SetMatrix4(string attribute, ref Matrix4 value)
        {
        }

        public new void SetVector4(string attribute, ref Vector4 value)
        {
        }
    }
}
