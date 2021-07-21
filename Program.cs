using System;
using System.Threading;

namespace CHIP_8
{
    static class Program
    {
        static void Main(string[] args)
        {
            Chip8.Init();
            Chip8.LoadGame("wipeoff");

            Graphics.Init();

            while (true)
            {
                Graphics.UpdateWindow();

                Input.GetInput();

                Chip8.EmulateCycle();

                if (Chip8.DrawFlag)
                {
                    Graphics.Draw();

                    Chip8.DrawFlag = false;
                }

                // Thread.Sleep(16);
            }
        }
    }
}
