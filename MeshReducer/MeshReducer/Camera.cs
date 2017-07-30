using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace MeshReducer
{
    class Camera
    {
        public Vector3 pos;
        public Vector3 dir;
        public Vector3 up;

        Vector3 step;
        float velocity;
        float mouse_speed = 0.001f;

        public Camera()
        {
            pos = new Vector3(0, 0, 0);
            dir = new Vector3(0, 0,-1);
            up  = new Vector3(0, 1, 0);

            step = new Vector3(0, 0, 0);
            velocity = 1.0f;
        }

        public void SetVelocity(float velocity)
        {
            this.velocity = velocity;
        }

        public void Update()
        {
            pos += step;

            step = new Vector3(0, 0, 0);
        }

        public void MoveUp(float dt)
        {
            step += dir * velocity * dt;
        }

        public void MoveDown(float dt)
        {
            step -= dir * velocity * dt;
        }

        public void MoveRight(float dt)
        {
            step += Vector3.Cross(dir, up) * velocity * dt;
        }

        public void MoveLeft(float dt)
        {
            step -= Vector3.Cross(dir, up) * velocity * dt;
        }

        public void RotateDir(float x, float y)
        {
            Matrix4x4 T1 = Matrix4x4.CreateRotationY(-x * mouse_speed);
            Matrix4x4 T2 = Matrix4x4.CreateFromAxisAngle(Vector3.Cross(dir, up), -y * mouse_speed);

            Vector4 dir4 = Vector4.Transform(new Vector4(dir, 0), Matrix4x4.Multiply(T1, T2));
            dir = Vector3.Normalize(new Vector3(dir4.X, dir4.Y, dir4.Z));
        }
    }
}
