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

            Clear();
        }

        public static void UpdateWindow()
        {
            SDL.SDL_Event e;

            while(SDL.SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        Program.Running = false;
                        break;

                    // TODO: keep aspect ratio of render context on window resize
                    /*case SDL.SDL_EventType.SDL_WINDOWEVENT:
                        switch (e.window.windowEvent)
                        {
                            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:

                                break;
                        }
                        break;*/
                }
            }
        }

        public static void Clear()
        {
            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(Renderer);
            SDL.SDL_RenderPresent(Renderer);
        }

        public static void Draw()
        {
            // clear before rendering (using Clear() renders twice per draw)
            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(Renderer);

            // render-context size
            int screenWidth, screenHeight;
            SDL.SDL_GetRendererOutputSize(Renderer, out screenWidth, out screenHeight);

            SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255);

            // Draws a white rect for every set pixel in Gfx memory
            SDL.SDL_Rect rect = new SDL.SDL_Rect();

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (Chip8.Gfx[y, x] != 0)
                    {
                        rect.w = screenWidth / 64;
                        rect.h = screenHeight / 32;
                        rect.x = x * (screenWidth / 64);
                        rect.y = y * (screenHeight / 32);

                        SDL.SDL_RenderFillRect(Renderer, ref rect);
                    }
                }
            }

            SDL.SDL_RenderPresent(Renderer);
        }

        public static void ShowFPS(long deltaTime)
        {
            Console.WriteLine("FPS: " + 1000 / deltaTime);
        }

        public static void Destroy()
        {
            SDL.SDL_DestroyWindow(Window);
            SDL.SDL_DestroyRenderer(Renderer);
            SDL.SDL_Quit();
        }
    }
}
