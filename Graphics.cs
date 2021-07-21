using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace CHIP_8
{
    static class Graphics
    {
        static IntPtr Window;
        static IntPtr Renderer;

        public static void Init()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL. Error: {0}", SDL.SDL_GetError());
                return;
            }

            Window = SDL.SDL_CreateWindow("Chip-8", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 640, 320, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            Renderer = SDL.SDL_CreateRenderer(Window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            // SDL.SDL_CreateWindowAndRenderer(640, 320, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE, out Window, out Renderer);

            Clear();
        }

        public static void UpdateWindow()
        {
            SDL.SDL_Event e;
        }

        public static void Clear()
        {
            // Console.WriteLine("Clearing");

            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(Renderer);
            SDL.SDL_RenderPresent(Renderer);
        }

        public static void Draw()
        {
            /*Console.WriteLine("Drawing");

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (Chip8.Gfx[y, x] == 1)
                        Console.Write("*");
                    else
                        Console.Write(" ");
                }
                Console.Write("\n");
            }

            Console.WriteLine();*/

            Clear();

            int screenWidth, screenHeight;
            SDL.SDL_GetRendererOutputSize(Renderer, out screenWidth, out screenHeight);

            SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (Chip8.Gfx[y, x] != 0)
                    {
                        SDL.SDL_Rect rect = new SDL.SDL_Rect();
                        rect.w = screenWidth / 64;
                        rect.h = screenHeight / 32;
                        rect.x = x * (screenWidth / 64);
                        rect.y = y * (screenHeight / 32);

                        SDL.SDL_RenderFillRect(Renderer, ref rect);
                        // SDL.SDL_RenderPresent(Renderer);
                    }
                }
            }

            SDL.SDL_RenderPresent(Renderer);
        }

        public static void Destroy()
        {
            SDL.SDL_DestroyWindow(Window);
            SDL.SDL_DestroyRenderer(Renderer);
            SDL.SDL_Quit();
        }

    }
}
