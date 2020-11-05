using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace otktest
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWinSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "My OpenTK Test",
                Flags = ContextFlags.ForwardCompatible
            };
        
            using var window = new Window(GameWindowSettings.Default, nativeWinSettings);
            window.Run();
        }
    }
}
