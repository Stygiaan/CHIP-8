using System;
using System.Diagnostics;
using System.Threading;

namespace CHIP_8
{
    static class Program
    {
        public static bool Running = true;
        private static Stopwatch Timer = new Stopwatch();

        static void Main(string[] args)
        {
            Chip8.Init();
            Chip8.LoadGame("blinky");

            Graphics.Init();
            // Audio.Init();

            while (Running)
            {
                Timer.Restart();

                Input.UpdateKeymap();

                Chip8.EmulateCycle();

                if (Chip8.DrawFlag)
                {
                    Graphics.Draw();

                    Chip8.DrawFlag = false;
                }

                Graphics.UpdateWindow();

                Thread.Sleep(2);

                Graphics.ShowFPS(Timer.ElapsedMilliseconds);
            }

            Graphics.Destroy();
            // Audio.Destroy();
        }
    }
}
