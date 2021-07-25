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
        /*private static Dictionary<SDL.SDL_Keycode, byte> KeyMap = new Dictionary<SDL.SDL_Keycode, byte>()
        {
            { SDL.SDL_Keycode.SDLK_x, 0 },
            { SDL.SDL_Keycode.SDLK_1, 1 },
            { SDL.SDL_Keycode.SDLK_2, 2 },
            { SDL.SDL_Keycode.SDLK_3, 3 },
            { SDL.SDL_Keycode.SDLK_q, 4 },
            { SDL.SDL_Keycode.SDLK_w, 5 },
            { SDL.SDL_Keycode.SDLK_e, 6 },
            { SDL.SDL_Keycode.SDLK_a, 7 },
            { SDL.SDL_Keycode.SDLK_s, 8 },
            { SDL.SDL_Keycode.SDLK_d, 9 },
            { SDL.SDL_Keycode.SDLK_y, 10 },
            { SDL.SDL_Keycode.SDLK_c, 11 },
            { SDL.SDL_Keycode.SDLK_4, 12 },
            { SDL.SDL_Keycode.SDLK_r, 13 },
            { SDL.SDL_Keycode.SDLK_f, 14 },
            { SDL.SDL_Keycode.SDLK_v, 15 }
        };*/

        public static void UpdateKeymap()
        {
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
            Chip8.Keys[0xA] = GetKey(SDL.SDL_Keycode.SDLK_y);
            Chip8.Keys[0xB] = GetKey(SDL.SDL_Keycode.SDLK_c);
            Chip8.Keys[0xC] = GetKey(SDL.SDL_Keycode.SDLK_4);
            Chip8.Keys[0xD] = GetKey(SDL.SDL_Keycode.SDLK_r);
            Chip8.Keys[0xE] = GetKey(SDL.SDL_Keycode.SDLK_f);
            Chip8.Keys[0xF] = GetKey(SDL.SDL_Keycode.SDLK_v);
        }

        private static byte GetKey(SDL.SDL_Keycode keycode)
        {
            int numkeys;
            byte keyPressed;
            byte scancode = (byte)SDL.SDL_GetScancodeFromKey(keycode);

            IntPtr keyboardState = SDL.SDL_GetKeyboardState(out numkeys);
            byte[] keys = new byte[numkeys];

            Marshal.Copy(keyboardState, keys, 0, numkeys);

            keyPressed = keys[scancode];

            return keyPressed;
        }

        public static byte AwaitAnyKey()
        {
            Console.Write("Keypress");

            while (true)
            {
                SDL.SDL_Event e;
                SDL.SDL_WaitEvent(out e);

                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        if (GetKey(SDL.SDL_Keycode.SDLK_x) != 0)
                            return 0x0;
                        if (GetKey(SDL.SDL_Keycode.SDLK_1) != 0)
                            return 0x1;
                        if (GetKey(SDL.SDL_Keycode.SDLK_2) != 0)
                            return 0x2;
                        if (GetKey(SDL.SDL_Keycode.SDLK_3) != 0)
                            return 0x3;
                        if (GetKey(SDL.SDL_Keycode.SDLK_q) != 0)
                            return 0x4;
                        if (GetKey(SDL.SDL_Keycode.SDLK_w) != 0)
                            return 0x5;
                        if (GetKey(SDL.SDL_Keycode.SDLK_e) != 0)
                            return 0x6;
                        if (GetKey(SDL.SDL_Keycode.SDLK_a) != 0)
                            return 0x7;
                        if (GetKey(SDL.SDL_Keycode.SDLK_s) != 0)
                            return 0x8;
                        if (GetKey(SDL.SDL_Keycode.SDLK_d) != 0)
                            return 0x9;
                        if (GetKey(SDL.SDL_Keycode.SDLK_y) != 0)
                            return 0xA;
                        if (GetKey(SDL.SDL_Keycode.SDLK_c) != 0)
                            return 0xB;
                        if (GetKey(SDL.SDL_Keycode.SDLK_4) != 0)
                            return 0xC;
                        if (GetKey(SDL.SDL_Keycode.SDLK_r) != 0)
                            return 0xD;
                        if (GetKey(SDL.SDL_Keycode.SDLK_f) != 0)
                            return 0xE;
                        if (GetKey(SDL.SDL_Keycode.SDLK_v) != 0)
                            return 0xF;
                        break;
                    case SDL.SDL_EventType.SDL_QUIT:
                        Program.Running = false;
                        return 0;
                }
            }
        }
    }
}
