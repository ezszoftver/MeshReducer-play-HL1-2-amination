using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using Tao.OpenGl;

namespace MeshReducer
{
    class MeshReducer
    {
        Mesh mesh;

        public MeshReducer(Mesh mesh)
        {
            this.mesh = new Mesh(mesh);
        }

        public Mesh Calc(int end_vertices_count)
        {
            while (end_vertices_count < mesh.GetVerticesCount() || mesh.GetVerticesCount() > 3)
            {
                EraseLine();
            }

            Mesh reduced_mesh = new Mesh();
            return reduced_mesh;
        }

        private void EraseLine()
        {
            ;
        }
    }
}
