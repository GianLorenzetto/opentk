using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace otktest
{
    public class Window : GameWindow
    {
        private readonly float[] _vertices = {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f,  0.5f, 0.0f
        };

        private readonly Shader _shader = new Shader();
        private int _vertexBufferObjectId;
        private int _vertexArrayObjectId;
        
        public Window(GameWindowSettings winSettings, NativeWindowSettings nativeSettings) : base(winSettings, nativeSettings) { }

        protected override void OnLoad()
        {
            _vertexArrayObjectId = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObjectId);            
            
            _vertexBufferObjectId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _shader.Create("shader.vert", "shader.frag");
            var attribIndex = _shader.GetAttribLocation("aPosition");

            GL.VertexAttribPointer(attribIndex, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(attribIndex);
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObjectId);
            
            _shader.Dispose();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            GL.BindVertexArray(_vertexArrayObjectId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            
            base.OnUpdateFrame(args);
        }
    }
}