using Castles.Data;
using Castles.Providers;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace Castles.Render
{
    public class Camera2D
    {
        private readonly float near = 1f;
        private readonly float far = 1000f;

        private Matrix4x4 viewMatrix;
        private Matrix4x4 projectionMatrix;

        private Vector3 position = new Vector3(0, 0, 0);
        private Vector3 lookDirection = new Vector3(0, 0, -1f);

        private float yaw;
        private float pitch;

        private float windowWidth;
        private float windowHeight;

        public event Action<Matrix4x4> ProjectionChanged;
        public event Action<Matrix4x4> ViewChanged;

        public Camera2D(SettingsProvider settingsProvider)
        {
            var settings = settingsProvider.Settings;
            windowWidth = settings.ScreenResolution.X;
            windowHeight = settings.ScreenResolution.Y;
            UpdatePerspectiveMatrix();
            UpdateViewMatrix();
        }

        public Matrix4x4 ViewMatrix => viewMatrix;
        public Matrix4x4 ProjectionMatrix => projectionMatrix;

        public Vector3 Position { get => position; set { position = value; UpdateViewMatrix(); } }
        public float AspectRatio => windowWidth / windowHeight;

        public float Yaw { get => yaw; set { yaw = value; UpdateViewMatrix(); } }
        public float Pitch { get => pitch; set { pitch = value; UpdateViewMatrix(); } }

        public void Update(float deltaSeconds)
        {
            //float sprintFactor = InputTracker.GetKey(Key.ShiftLeft)
            //        ? 2.5f
            //        : 1f;

            //Vector3 motionDir = Vector3.Zero;
            //if (InputTracker.GetKey(Key.A))
            //{
            //    motionDir += Vector3.UnitX;
            //}
            //if (InputTracker.GetKey(Key.D))
            //{
            //    motionDir += -Vector3.UnitX;
            //}
            //if (InputTracker.GetKey(Key.W))
            //{
            //    motionDir += Vector3.UnitZ;
            //}
            //if (InputTracker.GetKey(Key.S))
            //{
            //    motionDir += -Vector3.UnitZ;
            //}
            //if (InputTracker.GetKey(Key.ControlLeft))
            //{
            //    motionDir += -Vector3.UnitY;
            //}
            //if (InputTracker.GetKey(Key.Space))
            //{
            //    motionDir += Vector3.UnitY;
            //}

            //if (motionDir != Vector3.Zero)
            //{
            //    var direction = Vector3.Transform(
            //        Vector3.Transform(
            //            motionDir,
            //            Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, Pitch)),
            //        Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, -Yaw));

            //    position += direction * MoveSpeed * sprintFactor * deltaSeconds;
            //    UpdateViewMatrix();
            //}

            //Vector2 mouseDelta = InputTracker.MousePosition - _previousMousePos;
            //_previousMousePos = InputTracker.MousePosition;

            //if (InputTracker.GetMouseButton(MouseButton.Left) || InputTracker.GetMouseButton(MouseButton.Right))
            //{
            //    Yaw -= -mouseDelta.X * 0.01f;
            //    Pitch -= -mouseDelta.Y * 0.01f;
            //    Pitch = Clamp(Pitch, -1.55f, 1.55f);

            //    UpdateViewMatrix();
            //}
        }

        private float Clamp(float value, float min, float max)
        {
            return value > max
                ? max
                : value < min
                    ? min
                    : value;
        }

        public void WindowResized(float width, float height)
        {
            windowWidth = width;
            windowHeight = height;
            UpdatePerspectiveMatrix();
        }

        private void UpdatePerspectiveMatrix()
        {
            projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0, windowWidth, windowHeight, 0, near, far);
            //projectionMatrix = Matrix4x4.CreateOrthographic(windowWidth, windowHeight, near, far);
            ProjectionChanged?.Invoke(projectionMatrix);
        }

        private void UpdateViewMatrix()
        {
            Vector3 lookDir = GetLookDir();
            lookDirection = lookDir;
            viewMatrix = Matrix4x4.CreateLookAt(position, position + lookDirection, Vector3.UnitY);
            ViewChanged?.Invoke(viewMatrix);
        }

        private Vector3 GetLookDir()
        {
            Quaternion lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
            Vector3 lookDir = Vector3.Transform(-Vector3.UnitZ, lookRotation);
            return lookDir;
        }

        public CameraInfo GetCameraInfo() => new CameraInfo
        {
            CameraPosition_WorldSpace = position,
            CameraLookDirection = lookDirection
        };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CameraInfo
    {
        public Vector3 CameraPosition_WorldSpace;
        private float _padding1;
        public Vector3 CameraLookDirection;
        private float _padding2;
    }
}
