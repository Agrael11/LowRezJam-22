using LowRezJam22.Helpers;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;

namespace LowRezJam22
{
    public static class Starter
    {
        private static DebugProc _debugProcCallback = DebugCallback;
        private static GCHandle _debugProcCallbackHandle;

        private static void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            try
            {
                //if (severity == DebugSeverity.DontCare || severity == DebugSeverity.DebugSeverityNotification)
                    //return;
                string messageString = Marshal.PtrToStringAnsi(message, length);
                Logger.Levels errorLevel = Logger.Levels.Info;
                switch (severity)
                {
                    case DebugSeverity.DebugSeverityHigh:
                        errorLevel = Logger.Levels.Fatal;
                        break;
                    case DebugSeverity.DebugSeverityMedium:
                        errorLevel = Logger.Levels.Error;
                        break;
                    case DebugSeverity.DebugSeverityLow:
                        errorLevel = Logger.Levels.Warning;
                        break;
                    case DebugSeverity.DontCare:
                        errorLevel = Logger.Levels.Info;
                        break;
                    default:
                        errorLevel = Logger.Levels.Info;
                        break;
                }
                Logger.Log(errorLevel, messageString);
            }
            catch
            {
                Environment.Exit(-1);
            }
        }

        public static void Start()
        {
            Logger.StopAt = Logger.Levels.Fatal;
            Logger.FileLogAt = Logger.Levels.Error;
            Logger.LogFile = "Current.log";
            Logger.ConsoleLogAt = Logger.Levels.Info;

            GameWindowSettings gameWindowSettings = new();
            NativeWindowSettings nativeWindowSettings = new()
            {
                Size = new OpenTK.Mathematics.Vector2i(64*10, 64*10),
                WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed,
                Title = "LowRezJam 22"
            };


            _ = new Game(gameWindowSettings, nativeWindowSettings);

            _debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);
            GL.DebugMessageCallback(_debugProcCallback, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Fatal, "Could not create game");
                return; //Will never reach
            }

            Game.Instance.Init();

            Game.Instance.Run();
        }
    }
}