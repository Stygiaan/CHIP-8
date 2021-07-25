using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDL2;
using OpenTK.Audio.OpenAL;
using OpenTK;

namespace CHIP_8
{
    static class Audio
    {
        private const int AMPLITUDE = 28000;
        private const int SAMPLE_RATE = 44100;

        private static IntPtr audioPos;
        private static uint audioLength;

        public static object ContextHandle { get; private set; }

        public static void Init()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL-Audio. Error: {0}", SDL.SDL_GetError());
                return;
            }
        }

        public static unsafe void Beep()
        {
            /*SDL.SDL_AudioSpec Desired = new SDL.SDL_AudioSpec();
            SDL.SDL_AudioSpec Obtained;

            IntPtr sampleNum = (IntPtr)0;

            Desired.freq = SAMPLE_RATE;
            Desired.format = SDL.AUDIO_S16SYS;
            Desired.channels = 1;
            Desired.samples = 2048;
            Desired.callback = SDL_AudioCallback;
            Desired.userdata = sampleNum;

            SDL.SDL_OpenAudio(ref Desired, out Obtained);
            SDL.SDL_PauseAudio(0);
            SDL.SDL_Delay(200);
            SDL.SDL_CloseAudio();

            *//*Task T = new Task(() =>
            {
                SDL.SDL_OpenAudio(ref Desired, out Obtained);

                SDL.SDL_PauseAudio(0);
                SDL.SDL_Delay(500);
                SDL.SDL_CloseAudio();
            });

            T.Start();*/






            /*uint wavLength;
            IntPtr wavBuffer;
            SDL.SDL_AudioSpec wavSpec;

            SDL.SDL_LoadWAV("./Sound/beep.wav", out wavSpec, out wavBuffer, out wavLength);

            wavSpec.callback = SDL_AudioCallback;
            wavSpec.userdata = IntPtr.Zero;

            audioPos = wavBuffer;
            audioLength = wavLength;*/




            //Initialize
            var device = ALC.OpenDevice("OpenAL Soft");
            var context = ALC.CreateContext(device, (int*)null);

            ALC.MakeContextCurrent(context);

            var version = AL.Get(ALGetString.Version);
            var vendor = AL.Get(ALGetString.Vendor);
            var renderer = AL.Get(ALGetString.Renderer);
            Console.WriteLine(version);
            Console.WriteLine(vendor);
            Console.WriteLine(renderer);

            //Process
            int buffers, source;
            AL.GenBuffers(1, &buffers);
            AL.GenSources(1, &source);

            int sampleFreq = 44100;
            double dt = 2 * Math.PI / sampleFreq;
            double amp = 0.5;

            int freq = 440;
            var dataCount = sampleFreq / freq;

            var sinData = new short[dataCount];
            for (int i = 0; i < sinData.Length; ++i)
            {
                sinData[i] = (short)(amp * short.MaxValue * Math.Sin(i * dt * freq));
            }

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(sinData.Length);
            Marshal.Copy(sinData, 0, unmanagedPointer, sinData.Length);
            // Call unmanaged code
            // Marshal.FreeHGlobal(unmanagedPointer);

            AL.BufferData(buffers, ALFormat.Mono16, unmanagedPointer, sinData.Length * sizeof(short), sampleFreq);
            AL.Source(source, ALSourcei.Buffer, buffers);
            AL.Source(source, ALSourceb.Looping, true);

            AL.SourcePlay(source);

            

            ///Dispose
            /*if (context != IntPtr.Zero)
            {
                ALC.MakeContextCurrent(IntPtr.Zero);
                ALC.DestroyContext(context);
            }
            context = IntPtr.Zero;

            if (device != IntPtr.Zero)
            {
                ALC.CloseDevice(device);
            }
            device.*/
        }

        private static void SDL_AudioCallback(IntPtr userData, IntPtr stream, int bytes)
        {
            /*Int16[] buffer = new Int16[bytes];
            int length = bytes / 2;
            int sampleNum = (int)(userData);

            for (int i = 0; i < length; i++, sampleNum++)
            {
                double time = (double)(sampleNum) / (double)(SAMPLE_RATE);
                buffer[i] = (Int16)(AMPLITUDE * Math.Sin(2f * Math.PI * 441f * time));
            }

            Marshal.Copy(buffer, 0, stream, bytes);

            Console.WriteLine(buffer);*/





            /*if (audioLength == 0)
                return;

            byte[] buffer = new byte[bytes];

            Marshal.Copy(buffer, 0, stream, bytes);

            bytes = (bytes > audioLength ? audioLength : bytes);

            SDL.SDL_MixAudio(buffer, audioPos, (uint)bytes, SDL.SDL_MIX_MAXVOLUME);*/


        }

        public static void Destroy()
        {
            SDL.SDL_CloseAudio();
        }

        /*private static void Play(ref SDL.SDL_AudioSpec Desired, ref SDL.SDL_AudioSpec Obtained)
        {

            // SDL.SDL_CloseAudio();
        }*/
    }
}
