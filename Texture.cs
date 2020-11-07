using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace otktest
{
    public class Texture : IDisposable
    {
        private bool _disposedValue = false;
        private int _handle = -1;

        public void Create(string path)
        {
            if (_handle >= 0) throw new InvalidOperationException("Texture is already loaded"); 
            
            _handle = GL.GenTexture();
            Use();

            using Image<Rgba32> image = Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            var pixels = new List<byte>();
            for (var y = 0; y < image.Height; ++y)
            {
                var pixelRowSpan = image.GetPixelRowSpan(y);
                for (var x = 0; x < image.Width; ++x)
                {
                    var px = pixelRowSpan[x];
                    pixels.Add(px.R);
                    pixels.Add(px.G);
                    pixels.Add(px.B);
                    pixels.Add(px.A);
                }
            }
            
            GL.TexImage2D(
                TextureTarget.Texture2D, 
                0,
                PixelInternalFormat.Rgba,
                image.Width, image.Height,
                0, 
                PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                pixels.ToArray());
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            GL.DeleteProgram(_handle);
            _disposedValue = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Texture()
        {
            GL.DeleteProgram(_handle);
        }
    }
}