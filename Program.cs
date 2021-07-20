using System;
using System.Threading;

namespace CHIP_8
{
    class Program
    {
        static void Main(string[] args)
        {
            Chip8.Init();
            Chip8.LoadGame("BLITZ");

            while (true)
            {
                Chip8.EmulateCycle();

                Thread.Sleep(16);
            }
        }
    }
}
