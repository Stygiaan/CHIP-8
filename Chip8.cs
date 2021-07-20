using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8
{
    static class Chip8
    {
        static BinaryReader reader;
        static ushort Opcode;
        static byte[] Memory = new byte[4096];
        static byte[] V = new byte[16];
        static ushort I;
        static ushort Pc;
        static byte[] Gfx = new byte[64 * 32];
        static ushort[] Stack = new ushort[16];
        static ushort Sp;
        static byte[] Key = new byte[16];
        static byte DelayTimer;
        static byte SoundTimer;

        public static void Init()
        {
            Opcode = 0;
            Array.Clear(Memory, 0, Memory.Length);
            Array.Clear(V, 0, V.Length);
            I = 0;
            Pc = 0x200;
            Array.Clear(Gfx, 0, Gfx.Length);
            Array.Clear(Stack, 0, Stack.Length);
            Sp = 0;
            Array.Clear(Key, 0, Key.Length);

            // Load fontset
            for (int i = 0; i < 80; i++)
                Memory[i] = Fontset[i];

            // Reset Timers
            DelayTimer = 0;
            SoundTimer = 0;
        }

        public static void LoadGame(string title)
        {
            reader = new BinaryReader(File.Open("./Games/" + title, FileMode.Open));
            byte[] data = reader.ReadBytes((int)reader.BaseStream.Length);

            for (int i = 0; i < data.Length; i++)
            {
                Memory[i + 512] = data[i];
            }
        }

        public static void EmulateCycle()
        {
            FetchOpcode();
            ExecuteOpcode();

            UpdateTimers();
        }

        private static void FetchOpcode()
        {
            Opcode = (ushort)(Memory[Pc] << 8 | Memory[Pc + 1]);
        }
        private static void ExecuteOpcode()
        {
            switch (Opcode & 0xF000)
            {
                case 0xA000:
                    I = (ushort)(Opcode & 0x0FFF);
                    Pc += 2;
                    break;
                default:
                    Console.WriteLine("Unknown Opcode");
                    break;
            }
        }
        private static void UpdateTimers()
        {
            DelayTimer = (byte)(DelayTimer > 0 ? --DelayTimer : 0);

            if (SoundTimer == 1)
                Console.Beep();
            SoundTimer = (byte)(SoundTimer > 0 ? --DelayTimer : 0);
        }

        static byte[] Fontset =
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };
    }
}
