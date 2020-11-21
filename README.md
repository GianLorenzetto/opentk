# OpenTK Playground

Hi, this is my [OpenTK](https://opentk.net/index.html) playground.

Currently I'm creating my own implementation of the [Learn OpenTK tutorials](https://opentk.net/learn/index.html), but with some of my own tweaks and fixes.

The goal is to eventually create a simple, easy to use visualization engine.

If you're curious, look through the commit history to see the code for each chapter in the learning series.

## Requirements

You'll need [dotnet core installed](https://dotnet.microsoft.com/download/dotnet-core) for your platform.

## Running

To run, clone this repo then open a terminal at the top-level folder and execute the command

```c#
> dotnet run
```

A window will hopefully open showing a pretty boring scene right now. You can:
 
- translate the camera with `A, W, S, D, Space, LShift`,
- adjust camera yaw by moving the mouse,
- adjust the camera aspect (simulated zoom) with the scroll wheel,
- reset the camera position (not zoom though) with `R`.

That's it!

## Troubleshooting

If the app fails to start with any sort of OpenGL or OpenTK failures relating to the creation of the initial window, take a look at the `NativeWindowSettings` setup in `Program.cs`. Depending on your graphics hardware you may need to adjust the window configuration.
