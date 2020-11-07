using System;
using System.IO;
using System.Text;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL4.GL;
using ShaderType = OpenTK.Graphics.OpenGL4.ShaderType;

namespace otktest
{
    public class Shader : IDisposable
    {
        private bool _disposedValue = false;
        private int _handle = -1;

        public void Create(string vertexPath, string fragmentPath)
        {
            if (_handle >= 0) throw new InvalidOperationException("Shader is already loaded");
            
            var vertexShader = CreateShader(vertexPath, ShaderType.VertexShader);
            var fragmentShader = CreateShader(fragmentPath, ShaderType.FragmentShader);

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);
            
            GL.LinkProgram(_handle);
            
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        
        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public void SetInt(string name, int value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.Uniform1(location, value);
        }

        public void SetMatrix(string name, Matrix4 data)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.UniformMatrix4(location, true, ref data); 
        }
        
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }
        
        private string ReadShaderSource(string shaderPath)
        {
            using var reader = new StreamReader(shaderPath, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private int CreateShader(string shaderPath, ShaderType shaderType)
        {
            var shaderId = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderId, ReadShaderSource(shaderPath));

            GL.CompileShader(shaderId);

            var infoLogVert = GL.GetShaderInfoLog(shaderId);
            if (infoLogVert != System.String.Empty)
            {
                System.Console.WriteLine(infoLogVert);
                return -1;
            }

            return shaderId;
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

        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }
    }
}