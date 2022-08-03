using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LowRezJam22.Engine.Graphics
{
    internal class Shader
    {
        private int _handle;
        public int Handle { get { return _handle; } }

        public Shader(string vertexFilePath, string fragmentFilePath)
        {
            int VertexHandle;
            int FragmentHandle;
            int SuccessFlag;

            string VertexShaderSource;
            string FragmentShaderSource;

            try
            {
                VertexShaderSource = File.ReadAllText(vertexFilePath);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Levels.Error, $"Could not load vertex shader {vertexFilePath}\n{ex.Message}");
                return;
            }
            VertexHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexHandle, VertexShaderSource);
            GL.CompileShader(VertexHandle);
            GL.GetShader(VertexHandle, ShaderParameter.CompileStatus, out SuccessFlag);
            if (SuccessFlag == 0)
            {
                string InfoError = GL.GetProgramInfoLog(_handle);
                Logger.Log(Logger.Levels.Error, $"Could not load vertex shader {vertexFilePath}\n{InfoError}");
                return;
            }

            try
            {
                FragmentShaderSource = File.ReadAllText(fragmentFilePath);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Levels.Error, $"Could not load fragment shader {fragmentFilePath}\n{ex.Message}");
                return;
            }
            FragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentHandle, FragmentShaderSource);
            GL.CompileShader(FragmentHandle);
            GL.GetShader(FragmentHandle, ShaderParameter.CompileStatus, out SuccessFlag);
            if (SuccessFlag == 0)
            {
                string InfoError = GL.GetProgramInfoLog(_handle);
                Logger.Log(Logger.Levels.Error, $"Could not load fragment shader {fragmentFilePath}\n{InfoError}");
                return;
            }

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, VertexHandle);
            GL.AttachShader(_handle, FragmentHandle);

            GL.LinkProgram(_handle);
            GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out SuccessFlag);
            if (SuccessFlag == 0)
            {
                string InfoError = GL.GetProgramInfoLog(_handle);
                Logger.Log(Logger.Levels.Error, $"Could not link shader\n{InfoError}");
                return;
            }

            GL.DetachShader(_handle, VertexHandle);
            GL.DetachShader(_handle, FragmentHandle);
            GL.DeleteBuffer(VertexHandle);
            GL.DeleteBuffer(FragmentHandle);
        }

        internal Shader()
        {

        }

        public void UseShader()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttributeLocation(string attribute)
        {
            UseShader();
            return GL.GetAttribLocation(_handle, attribute);
        }

        public int GetUniformLocation(string uniform)
        {
            UseShader();
            return GL.GetUniformLocation(_handle, uniform);
        }

        public void SetInt(string attribute, int value)
        {
            int location = GetUniformLocation(attribute);
            GL.Uniform1(location, value);
        }

        public void SetFloat(string attribute, float value)
        {
            int location = GetUniformLocation(attribute);
            GL.Uniform1(location, value);
        }

        public void SetDouble(string attribute, double value)
        {
            int location = GetUniformLocation(attribute);
            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string attribute, ref Matrix4 value)
        {
            int location = GetUniformLocation(attribute);
            GL.UniformMatrix4(location, true, ref value);
        }

        public void SetVector4(string attribute, ref Vector4 value)
        {
            int location = GetUniformLocation(attribute);
            GL.Uniform4(location, ref value);
        }
    }
}
