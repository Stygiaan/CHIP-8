using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace CHIP_8
{
    static class Input
    {
        public static void GetInput()
        {
            Array.Clear(Chip8.Keys, 0, Chip8.Keys.Length);

            SDL.SDL_PumpEvents();

            Chip8.Keys[0x0] = GetKey(SDL.SDL_Keycode.SDLK_x);
            Chip8.Keys[0x1] = GetKey(SDL.SDL_Keycode.SDLK_1);
            Chip8.Keys[0x2] = GetKey(SDL.SDL_Keycode.SDLK_2);
            Chip8.Keys[0x3] = GetKey(SDL.SDL_Keycode.SDLK_3);
            Chip8.Keys[0x4] = GetKey(SDL.SDL_Keycode.SDLK_q);
            Chip8.Keys[0x5] = GetKey(SDL.SDL_Keycode.SDLK_w);
            Chip8.Keys[0x6] = GetKey(SDL.SDL_Keycode.SDLK_e);
            Chip8.Keys[0x7] = GetKey(SDL.SDL_Keycode.SDLK_a);
            Chip8.Keys[0x8] = GetKey(SDL.SDL_Keycode.SDLK_s);
            Chip8.Keys[0x9] = GetKey(SDL.SDL_Keycode.SDLK_d);
            Chip8.Keys[0xA] = GetKey(SDL.SDL_Keycode.SDLK_a);
            Chip8.Keys[0xB] = GetKey(SDL.SDL_Keycode.SDLK_c);
            Chip8.Keys[0xC] = GetKey(SDL.SDL_Keycode.SDLK_4);
            Chip8.Keys[0xD] = GetKey(SDL.SDL_Keycode.SDLK_r);
            Chip8.Keys[0xE] = GetKey(SDL.SDL_Keycode.SDLK_f);
            Chip8.Keys[0xF] = GetKey(SDL.SDL_Keycode.SDLK_v);

            // Console.WriteLine(GetKey(SDL.SDL_Keycode.SDLK_0));
        }

        private static byte GetKey(SDL.SDL_Keycode _keycode)
        {
            int numkeys;
            byte keyPressed;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out numkeys);
            byte[] keys = new byte[numkeys];
            byte keycode = (byte)SDL.SDL_GetScancodeFromKey(_keycode);

            Marshal.Copy(origArray, keys, 0, numkeys);

            keyPressed = keys[keycode];

            if(keyPressed != 0)
                Console.WriteLine(keycode);

            return keyPressed;
        }

        public static byte AwaitAnyKey()
        {
            while (true)
            {
                if (GetKey(SDL.SDL_Keycode.SDLK_x) == 1)
                    return 0x0;
                if (GetKey(SDL.SDL_Keycode.SDLK_1) == 1)
                    return 0x1;
                if (GetKey(SDL.SDL_Keycode.SDLK_2) == 1)
                    return 0x2;
                if (GetKey(SDL.SDL_Keycode.SDLK_3) == 1)
                    return 0x3;
                if (GetKey(SDL.SDL_Keycode.SDLK_q) == 1)
                    return 0x4;
                if (GetKey(SDL.SDL_Keycode.SDLK_w) == 1)
                    return 0x5;
                if (GetKey(SDL.SDL_Keycode.SDLK_e) == 1)
                    return 0x6;
                if (GetKey(SDL.SDL_Keycode.SDLK_a) == 1)
                    return 0x7;
                if (GetKey(SDL.SDL_Keycode.SDLK_s) == 1)
                    return 0x8;
                if (GetKey(SDL.SDL_Keycode.SDLK_d) == 1)
                    return 0x9;
                if (GetKey(SDL.SDL_Keycode.SDLK_y) == 1)
                    return 0xA;
                if (GetKey(SDL.SDL_Keycode.SDLK_c) == 1)
                    return 0xB;
                if (GetKey(SDL.SDL_Keycode.SDLK_4) == 1)
                    return 0xC;
                if (GetKey(SDL.SDL_Keycode.SDLK_r) == 1)
                    return 0xD;
                if (GetKey(SDL.SDL_Keycode.SDLK_f) == 1)
                    return 0xE;
                if (GetKey(SDL.SDL_Keycode.SDLK_v) == 1)
                    return 0xF;
            }
        }
    }
}
