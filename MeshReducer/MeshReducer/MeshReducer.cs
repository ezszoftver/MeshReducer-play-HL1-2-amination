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
        class Edge
        {
            public Vector3 a;
            public Vector3 b;

            public Edge(Vector3 a, Vector3 b)
            {
                this.a = a;
                this.b = b;
            }

            public float GetLength()
            {
                return Vector3.Distance(a, b);
            }
        }

        Mesh mesh;

        public MeshReducer(Mesh mesh)
        {
            this.mesh = mesh;
        }

        Edge SearchMinEdge()
        {
            Edge min_edge = new Edge(new Vector3(-1000000.0f, -1000000.0f, -1000000.0f), new Vector3(1000000.0f, 1000000.0f, 1000000.0f));
            Edge edge;
            Vector3 A;
            Vector3 B;
            Vector3 C;

            foreach (Mesh.Material material in mesh.materials)
            {
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {
                    if (material.vertices[v + 0] == null
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }

                    A = material.vertices[v + 0].vertex;
                    B = material.vertices[v + 1].vertex;
                    C = material.vertices[v + 2].vertex;

                    // AB
                    edge = new Edge(A, B);
                    if (edge.GetLength() < min_edge.GetLength()) { min_edge = edge; }
                    // BC
                    edge = new Edge(B, C);
                    if (edge.GetLength() < min_edge.GetLength()) { min_edge = edge; }
                    // CA
                    edge = new Edge(C, A);
                    if (edge.GetLength() < min_edge.GetLength()) { min_edge = edge; }
                }
            }

            return min_edge;
        }

        bool IsEqual(Vector3 a, Vector3 b)
        {
            if (Vector3.Distance(a, b) < 0.0001f) { return true; }
            return false;
        }

        int NumEdgePointsInTriangle(Edge edge, Vector3 A, Vector3 B, Vector3 C, out bool hit_a, out bool hit_b, out bool hit_c)
        {
            int count = 0;
            hit_a = false;
            hit_b = false;
            hit_c = false;

            if (IsEqual(edge.a, A)) { hit_a = true; }
            if (IsEqual(edge.a, B)) { hit_b = true; }
            if (IsEqual(edge.a, C)) { hit_c = true; }

            if (IsEqual(edge.b, A)) { hit_a = true; }
            if (IsEqual(edge.b, B)) { hit_b = true; }
            if (IsEqual(edge.b, C)) { hit_c = true; }

            if (hit_a) { count++; }
            if (hit_b) { count++; }
            if (hit_c) { count++; }

            return count;
        }

        public void EraseLine()
        {
            Edge edge = SearchMinEdge();

            Vector3 new_vertex = (edge.a + edge.b) / 2.0f;

            Vector3 VA;
            Vector3 VB;
            Vector3 VC;
            Vector2 TA;
            Vector2 TB;
            Vector2 TC;
            bool hit_a, hit_b, hit_c;

            Vector2 new_textcoords = new Vector2(0, 0);
            Mesh.Material new_material = mesh.materials[0];
            bool have_textcoords = false;

            foreach (Mesh.Material material in mesh.materials)
            {
                // textcoords
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {
                    if (material.vertices[v + 0] == null
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }

                    VA = material.vertices[v + 0].vertex;
                    VB = material.vertices[v + 1].vertex;
                    VC = material.vertices[v + 2].vertex;
                    TA = material.vertices[v + 0].textcoords;
                    TB = material.vertices[v + 1].textcoords;
                    TC = material.vertices[v + 2].textcoords;

                    if (NumEdgePointsInTriangle(edge, VA, VB, VC, out hit_a, out hit_b, out hit_c) >= 2)
                    {
                        if (hit_a && hit_b) { new_textcoords = (TA + TB) / 2.0f; }
                        if (hit_b && hit_c) { new_textcoords = (TB + TC) / 2.0f; }
                        if (hit_c && hit_a) { new_textcoords = (TC + TA) / 2.0f; }
                        have_textcoords = true;
                        new_material = material;
                        goto end_textcoords;
                    }
                }
            }

            if (false == have_textcoords) { return; }
        end_textcoords:

            // vertices
            foreach (Mesh.Material material in mesh.materials)
            {
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {

                    if (material.vertices[v + 0] == null
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }

                    VA = material.vertices[v + 0].vertex;
                    VB = material.vertices[v + 1].vertex;
                    VC = material.vertices[v + 2].vertex;
                    TA = material.vertices[v + 0].textcoords;
                    TB = material.vertices[v + 1].textcoords;
                    TC = material.vertices[v + 2].textcoords;

                    int count = NumEdgePointsInTriangle(edge, VA, VB, VC, out hit_a, out hit_b, out hit_c);
                    if (count > 0)
                    {
                        if (hit_a)
                        {
                            material.vertices[v + 0].vertex = new_vertex;
                            if (count >= 2 && material == new_material) material.vertices[v + 0].textcoords = new_textcoords;
                        }
                        if (hit_b)
                        {
                            material.vertices[v + 1].vertex = new_vertex;
                            if (count >= 2 && material == new_material) material.vertices[v + 1].textcoords = new_textcoords;
                        }
                        if (hit_c)
                        {
                            material.vertices[v + 2].vertex = new_vertex;
                            if (count >= 2 && material == new_material) material.vertices[v + 2].textcoords = new_textcoords;
                        }
                    }
                }
            }

            // delete not true triangles
            foreach (Mesh.Material material in mesh.materials)
            {
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {
                    if (material.vertices[v + 0] == null
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }

                    VA = material.vertices[v + 0].vertex;
                    VB = material.vertices[v + 1].vertex;
                    VC = material.vertices[v + 2].vertex;

                    if (IsEqual(VA, VB) || IsEqual(VB, VC) || IsEqual(VC, VA))
                    {
                        material.vertices[v + 0].matrices.Clear();
                        material.vertices[v + 0].matrices = null;
                        material.vertices[v + 0] = null;

                        material.vertices[v + 1].matrices.Clear();
                        material.vertices[v + 1].matrices = null;
                        material.vertices[v + 1] = null;

                        material.vertices[v + 2].matrices.Clear();
                        material.vertices[v + 2].matrices = null;
                        material.vertices[v + 2] = null;
                    }
                }
            }
        }
    }
}
