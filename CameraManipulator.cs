using System;
using Microsoft.Win32.SafeHandles;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace otktest
{
    public class CameraManipulator
    {
        private const float ZoomIncrement = 1f;

        private Vector2 _lastPos;
        private bool _firstMove = true;
        private Camera? _camera;
        private float _cameraSpeed;
        private float _cameraSensitivity;

        public void Create(Camera camera, float cameraSPeed = 1.0f, float cameraSensitivity = 0.2f)
        {
            _camera = camera;
            _cameraSpeed = cameraSPeed;
            _cameraSensitivity = cameraSensitivity;
        }
        
        public void Update(KeyboardState ks, MouseState ms, float timeDelta)
        {
            if (_camera == null) throw new InvalidOperationException("Camera manipulator not initialised. 'Create(..)' must be called first with a valid Camera.");
            
            if (ks.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * _cameraSpeed * timeDelta;
            }

            if (ks.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * _cameraSpeed * timeDelta;
            }

            if (ks.IsKeyDown(Keys.A))
            {
                _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _cameraSpeed * timeDelta; //Left
            }

            if (ks.IsKeyDown(Keys.D))
            {
                _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _cameraSpeed * timeDelta; //Right
            }

            if (ks.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * _cameraSpeed * timeDelta;
            }

            if (ks.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * _cameraSpeed * timeDelta;
            }

            if (_firstMove)
            {
                _lastPos = new Vector2(ms.X, ms.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = ms.X - _lastPos.X;
                var deltaY = ms.Y - _lastPos.Y;
                _lastPos = new Vector2(ms.X, ms.Y);
                
                _camera.Yaw += deltaX * _cameraSensitivity;
                _camera.Pitch -= deltaY * _cameraSensitivity;
            }
        }
        
        public void Zoom(bool increaseZoom)
        {
            if (_camera == null) throw new InvalidOperationException("Camera manipulator not initialised. 'Create(..)' must be called first with a valid Camera.");

            _camera.Fov += increaseZoom ? ZoomIncrement : -ZoomIncrement;
        }
    }
}