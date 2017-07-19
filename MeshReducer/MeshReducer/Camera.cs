using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GlmNet;

namespace MeshReducer.Camera
{
    class Camera
    {
        public vec3 pos;
        public vec3 dir;
        public vec3 up;

        vec3 step;
        float velocity;
        float mouse_speed = 0.001f;

        public Camera()
        {
            pos = new vec3(0, 0, 0);
            dir = new vec3(0, 0,-1);
            up  = new vec3(0, 1, 0);

            step = new vec3(0, 0, 0);
            velocity = 1.0f;
        }

        public void SetVelocity(float velocity)
        {
            this.velocity = velocity;
        }

        public void Update()
        {
            pos += step;

            step = new vec3(0, 0, 0);
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
            step += glm.cross(dir, up) * velocity * dt;
        }

        public void MoveLeft(float dt)
        {
            step -= glm.cross(dir, up) * velocity * dt;
        }

        public void RotateDir(float x, float y)
        {
            mat4 T1 = glm.rotate(-x * mouse_speed, new vec3(0, 1, 0));
            mat4 T2 = glm.rotate(y * mouse_speed, glm.cross(dir, up));

            dir = new vec3(T2 * T1 * new vec4(dir, 0));
            dir = glm.normalize(dir);
        }
    }
}
