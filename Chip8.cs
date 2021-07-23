using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8
{
    static class Chip8
    {
        static BinaryReader Reader;
        static Random Rand = new Random();
        static ushort Opcode;
        static byte[] Memory = new byte[4096];
        static byte[] V = new byte[16];
        static ushort I;
        static ushort Pc;
        public static byte[,] Gfx = new byte[32, 64];
        public static bool DrawFlag = false;
        static ushort[] Stack = new ushort[16];
        static ushort Sp;
        public static byte[] Keys = new byte[16];
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
            Array.Clear(Keys, 0, Keys.Length);

            // Load fontset
            for (int i = 0; i < 80; i++)
                Memory[i + 80] = Fontset[i];

            // Reset Timers
            DelayTimer = 0;
            SoundTimer = 0;
        }

        public static void LoadGame(string title)
        {
            Reader = new BinaryReader(File.Open("./Games/" + title, FileMode.Open));
            byte[] data = Reader.ReadBytes((int)Reader.BaseStream.Length);

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
            Opcode = (ushort)((Memory[Pc] << 8) | Memory[Pc + 1]);
        }
        private static void ExecuteOpcode()
        {
            switch (Opcode & 0xF000)
            {
                case 0x0000:
                    switch (Opcode & 0x000F)
                    {
                        // 00E0 - Clears the screen. 
                        case 0x0000:
                            Graphics.Clear();
                            Pc += 2;
                            break;
                        // 00EE - Returns from a subroutine. 
                        case 0x000E:
                            Stack[Sp] = 0;
                            Sp--;
                            Pc = Stack[Sp] += 2;
                            break;
                        default:
                            break;
                    }
                    break;

                // 1NNN - Jumps to address NNN. 
                case 0x1000:
                    Pc = (ushort)(Opcode & 0x0FFF);
                    break;

                // 2NNN - Calls subroutine at NNN. 
                case 0x2000:
                    Stack[Sp] = Pc;
                    Sp++;
                    Pc = (ushort)(Opcode & 0x0FFF);
                    break;

                // 3XNN - Skips the next instruction if VX equals NN. (Usually the next instruction is a jump to skip a code block); 
                case 0x3000:
                    if (V[(Opcode & 0x0F00) >> 8] == (Opcode & 0x00FF))
                        Pc += 4;
                    else
                        Pc += 2;
                    break;

                // 4XNN - Skips the next instruction if VX does not equal NN. (Usually the next instruction is a jump to skip a code block); 
                case 0x4000:
                    if (V[(Opcode & 0x0F00) >> 8] != (Opcode & 0x00FF))
                        Pc += 4;
                    else
                        Pc += 2;
                    break;

                // 5XY0 - Skips the next instruction if VX equals VY. (Usually the next instruction is a jump to skip a code block); 
                case 0x5000:
                    if (V[(Opcode & 0x0F00) >> 8] == V[(Opcode & 0x00F0) >> 4])
                        Pc += 4;
                    else
                        Pc += 2;
                    break;

                // 6XNN - Sets VX to NN. 
                case 0x6000:
                    V[(Opcode & 0x0F00) >> 8] = (byte)(Opcode & 0x00FF);
                    Pc += 2;
                    break;

                // 7XNN - Adds NN to VX. (Carry flag is not changed); 
                case 0x7000:
                    V[(Opcode & 0x0F00) >> 8] += (byte)(Opcode & 0x00FF);
                    Pc += 2;
                    break;

                case 0x8000:
                    switch (Opcode & 0x000F)
                    {
                        // 8XY0 - Sets VX to the value of VY. 
                        case 0x0000:
                            V[(Opcode & 0x0F00) >> 8] = V[(Opcode & 0x00F0) >> 4];
                            Pc += 2;
                            break;

                        // 8XY1 - Sets VX to VX or VY. (Bitwise OR operation); 
                        case 0x0001:
                            V[(Opcode & 0x0F00) >> 8] = (byte)(V[(Opcode & 0x0F00) >> 8] | V[(Opcode & 0x00F0) >> 4]);
                            Pc += 2;
                            break;

                        // 8XY2 - Sets VX to VX and VY. (Bitwise AND operation); 
                        case 0x0002:
                            V[(Opcode & 0x0F00) >> 8] = (byte)(V[(Opcode & 0x0F00) >> 8] & V[(Opcode & 0x00F0) >> 4]);
                            Pc += 2;
                            break;

                        // 8XY3 - Sets VX to VX xor VY. 
                        case 0x0003:
                            V[(Opcode & 0x0F00) >> 8] = (byte)(V[(Opcode & 0x0F00) >> 8] ^ V[(Opcode & 0x00F0) >> 4]);
                            Pc += 2;
                            break;

                        // 8XY4 - Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there is not. 
                        case 0x0004:
                            if ((V[(Opcode & 0x00F0) >> 4] + V[(Opcode & 0x0F00) >> 8]) > 0xFF)
                                V[0xF] = 1;
                            else
                                V[0xF] = 0;
                            V[(Opcode & 0x0F00) >> 8] += V[(Opcode & 0x00F0) >> 4];
                            Pc += 2;
                            break;

                        // 8XY5 - VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there is not. 
                        case 0x0005:
                            if ((V[(Opcode & 0x0F00) >> 8] - V[(Opcode & 0x00F0) >> 4]) < 0x00)
                                V[0xF] = 0;
                            else
                                V[0xF] = 1;
                            V[(Opcode & 0x0F00) >> 8] -= V[(Opcode & 0x00F0) >> 4];
                            Pc += 2;
                            break;

                        // 8XY6 - Stores the least significant bit of VX in VF and then shifts VX to the right by 1. 
                        case 0x0006:
                            int lsbPosition = BitOperations.TrailingZeroCount(V[(Opcode & 0x0F00) >> 8]);

                            V[0xF] = (byte)((0b0000_0001 << lsbPosition) & V[(Opcode & 0x0F00) >> 8]);
                            V[(Opcode & 0x0F00) >> 8] >>= 1;

                            Pc += 2;
                            break;

                        // 8XY7 - Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there is not. 
                        case 0x0007:
                            if ((V[(Opcode & 0x00F0) >> 4] - V[(Opcode & 0x0F00) >> 8]) < 0x0)
                                V[0xF] = 0;
                            else
                                V[0xF] = 1;
                            V[(Opcode & 0x0F00) >> 8] = (byte)(V[(Opcode & 0x00F0) >> 4] - V[(Opcode & 0x0F00) >> 8]);
                            Pc += 2;
                            break;

                        // 8XYE - Stores the most significant bit of VX in VF and then shifts VX to the left by 1
                        case 0x000E:
                            int msbPosition = BitOperations.LeadingZeroCount(V[(Opcode & 0x0F00) >> 8]);

                            V[0xF] = (byte)((0b1000_0000 >> msbPosition) & V[(Opcode & 0x0F00) >> 8]);
                            V[(Opcode & 0x0F00) >> 8] <<= 1;

                            Pc += 2;
                            break;
                    }
                    break;

                // 9XY0 - Skips the next instruction if VX does not equal VY. (Usually the next instruction is a jump to skip a code block); 
                case 0x9000:
                    if (V[(Opcode & 0x0F00) >> 8] != V[(Opcode & 0x00F0) >> 4])
                        Pc += 4;
                    else
                        Pc += 2;
                    break;

                // ANNN - Sets I to the address NNN. 
                case 0xA000:
                    I = (ushort)(Opcode & 0x0FFF);
                    Pc += 2;
                    break;

                // BNNN - Jumps to the address NNN plus V0.
                case 0xB000:
                    Pc = (ushort)((Opcode & 0x0FFF) + V[0x0]);
                    break;

                // CXNN - Sets VX to the result of a bitwise and operation on a random number (Typically: 0 to 255) and NN. 
                case 0xC000:
                    V[(Opcode & 0x0F00) >> 8] = (byte)(Rand.Next(0, 255) & 0x00FF);
                    Pc += 2;
                    break;

                
                // DXYN - Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N+1 pixels.
                // Each row of 8 pixels is read as bit-coded starting from memory location I; I value does not change after the execution of this instruction.
                // As described above, VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, and to 0 if that does not happen
                case 0xD000:
                    ushort x = V[(Opcode & 0x0F00) >> 8];
                    ushort y = V[(Opcode & 0x00F0) >> 4];
                    ushort height = (ushort)(Opcode & 0x000F);
                    ushort pixelRow;

                    V[0xF] = 0;

                    for (int yLine = 0; yLine < height; yLine++)
                    {
                        pixelRow = Memory[I + yLine];

                        for (int xLine = 0; xLine < 8; xLine++)
                        {
                            if ((pixelRow & (0b1000_0000 >> xLine)) != 0)
                            {
                                if ((y + yLine) < Gfx.GetLength(0) && (x + xLine) < Gfx.GetLength(1))
                                {
                                    if (Gfx[y + yLine, x + xLine] == 1)
                                        V[0xF] = 1;
                                    Gfx[y + yLine, x + xLine] ^= 1;
                                }
                            }
                        }
                    }

                    DrawFlag = true;
                    Pc += 2;
                    break;

                case 0xE000:
                    switch (Opcode & 0x000F)
                    {
                        // EX9E - Skips the next instruction if the key stored in VX is pressed. (Usually the next instruction is a jump to skip a code block); 
                        case 0x000E:
                            if (Keys[V[(Opcode & 0x0F00) >> 8]] != 0)
                                Pc += 4;
                            else
                                Pc += 2;
                            break;

                        // EXA1 - Skips the next instruction if the key stored in VX is not pressed. (Usually the next instruction is a jump to skip a code block); 
                        case 0x0001:
                            if (Keys[V[(Opcode & 0x0F00) >> 8]] == 0)
                                Pc += 4;
                            else
                                Pc += 2;
                            break;
                    }
                    break;

                case 0xF000:
                    switch (Opcode & 0x00FF)
                    {
                        // FX07 - Sets VX to the value of the delay timer. 
                        case 0x0007:
                            V[(Opcode & 0x0F00) >> 8] = DelayTimer;
                            Pc += 2;
                            break;

                        // FX0A - A key press is awaited, and then stored in VX. (Blocking Operation. All instruction halted until next key event); 
                        case 0x000A:
                            V[(Opcode & 0x0F00) >> 8] = Input.AwaitAnyKey();
                            Pc += 2;
                            break;

                        // FX15 - Sets the delay timer to VX. 
                        case 0x0015:
                            DelayTimer = V[(Opcode & 0x0F00) >> 8];
                            Pc += 2;
                            break;

                        // FX18 - Sets the sound timer to VX. 
                        case 0x0018:
                            SoundTimer = V[(Opcode & 0x0F00) >> 8];
                            Pc += 2;
                            break;

                        // FX1E - Adds VX to I. VF is not affected.
                        case 0x001E:
                            I += V[(Opcode & 0x0F00) >> 8];
                            Pc += 2;
                            break;

                        // FX29 - Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font. 
                        case 0x0029:
                            I = (ushort)(80 + ((V[(Opcode & 0x0F00) >> 8]) * 5));
                            Pc += 2;
                            break;

                        // FX33 - Stores the binary-coded decimal representation of VX, with the most significant of three digits at the address in I,
                        // the middle digit at I plus 1, and the least significant digit at I plus 2. (In other words, take the decimal representation of VX,
                        // place the hundreds digit in memory at location in I, the tens digit at location I+1, and the ones digit at location I+2.); 
                        case 0x0033:
                            Memory[I] = (byte)(V[(Opcode & 0x0F00) >> 8] / 100);
                            Memory[I + 1] = (byte)((V[(Opcode & 0x0F00) >> 8] / 10) % 10);
                            Memory[I + 2] = (byte)((V[(Opcode & 0x0F00) >> 8] % 100) % 10);
                            Pc += 2;
                            break;

                        // FX55 - Stores V0 to VX (including VX) in memory starting at address I. The offset from I is increased by 1 for each value written,
                        // but I itself is left unmodified.
                        case 0x0055:
                            for (int i = 0; i <= ((Opcode & 0x0F00) >> 8); i++)
                            {
                                Memory[I + i] = V[i];
                            }
                            Pc += 2;
                            break;

                        // FX65 - Fills V0 to VX (including VX) with values from memory starting at address I. The offset from I is increased by 1 for each value written,
                        // but I itself is left unmodified.
                        case 0x0065:
                            for (int i = 0; i <= ((Opcode & 0x0F00) >> 8); i++)
                            {
                                V[i] = Memory[I + i];
                            }
                            Pc += 2;
                            break;
                    }
                    break;

                // Undefined Opcode
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
            SoundTimer = (byte)(SoundTimer > 0 ? --SoundTimer : 0);
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
