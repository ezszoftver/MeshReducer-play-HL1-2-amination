using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Numerics;
using Tao.OpenGl;

namespace MeshReducer
{
    class MeshReducer
    {
        class Edge : IEquatable<Edge>
        {
            public Vector3 a;
            public Vector3 b;

            public Edge(Vector3 a, Vector3 b)
            {
                this.a = new Vector3(a.X, a.Y, a.Z);
                this.b = new Vector3(b.X, b.Y, b.Z);
            }

            public bool Equals(Edge other)
            {
                if (   (a.Equals(other.a) && b.Equals(other.b))
                    || (a.Equals(other.b) && b.Equals(other.a)))
                {
                    return true;
                }

                return false;
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

        //int GetVertices(List<Triangle> triangles)
        //{
        //    return (triangles.Count * 3);
        //}

        int num_vertices = 0;

        int PartGetVertices()
        {
            //int num = 0;
            //
            //for (int i = 0; i < SIZE + 1; i++)
            //{
            //    for (int j = 0; j < SIZE + 1; j++)
            //    {
            //        for (int k = 0; k < SIZE + 1; k++)
            //        {
            //            num += part[i, j, k].Count;
            //        }
            //    }
            //}
            //
            //return (num * 3);
            return num_vertices;
        }

        bool SearchMinEdge(/*List<Triangle> triangles,*/ int num, out Edge min_edge)
        {
            bool is_ok = false;
            min_edge = new Edge(new Vector3(+1000000000.0f, +1000000.0f, +1000000000.0f), new Vector3(-1000000000.0f, -1000000000.0f, -1000000000.0f));
            Edge edge;
            Vector3 A;
            Vector3 B;
            Vector3 C;

            if (/*triangles.Count == 0*/PartGetVertices() < 3)
            {
                //System.Console.WriteLine("error triangles 0");
                return false;
            }

            //if (num > triangles.Count || num == -1) num = triangles.Count;
            //int step = (int)((float)triangles.Count / (float)num);
            //if (step <= 0) step = 1;

            Random rnd = new Random();
            if (num > /*triangles.Count*/PartGetVertices() || num == -1)
            {
                num = /*triangles.Count*/PartGetVertices();
            }

            //for (int ii = 0; ii < num; ii++)
            int ii = 0;
            while(ii < num)
            {
                //int i = rnd.Next(0, triangles.Count - 1);
                //Triangle tri = triangles[i];

                int i, j, k;
                do
                {
                    i = rnd.Next(0, SIZE);
                    j = rnd.Next(0, SIZE);
                    k = rnd.Next(0, SIZE);
                }
                while (part[i, j, k].Count == 0);
                int l = rnd.Next(0, part[i, j, k].Count - 1);
                Triangle tri = part[i, j, k][l];

                ii++;

                A = tri.A.vertex;
                B = tri.B.vertex;
                C = tri.C.vertex;

                // AB
                edge = new Edge(A, B);
                if (edge.GetLength() < min_edge.GetLength() /*&& edge.GetLength() > 0.001f*/)
                {
                    //if (false == bad_edges.Contains(edge))
                    {
                        min_edge = edge;
                        is_ok = true;
                    }
                }
                // BC
                edge = new Edge(B, C);
                if (edge.GetLength() < min_edge.GetLength() /*&& edge.GetLength() > 0.001f*/)
                {
                    //if (false == bad_edges.Contains(edge))
                    {
                        min_edge = edge;
                        is_ok = true;
                    }
                }
                // CA
                edge = new Edge(C, A);
                if (edge.GetLength() < min_edge.GetLength() /*&& edge.GetLength() > 0.001f*/)
                {
                    //if (false == bad_edges.Contains(edge))
                    {
                        min_edge = edge;
                        is_ok = true;
                    }
                }
            }

            return is_ok;
        }

        int NumEdgePointsInTriangle(Edge edge, Vector3 A, Vector3 B, Vector3 C, out bool hit_a, out bool hit_b, out bool hit_c)
        {
            int count = 0;
            hit_a = false;
            hit_b = false;
            hit_c = false;

            if (edge.a.Equals(A)) { hit_a = true; }
            if (edge.a.Equals(B)) { hit_b = true; }
            if (edge.a.Equals(C)) { hit_c = true; }

            if (edge.b.Equals(A)) { hit_a = true; }
            if (edge.b.Equals(B)) { hit_b = true; }
            if (edge.b.Equals(C)) { hit_c = true; }

            if (hit_a) { count++; }
            if (hit_b) { count++; }
            if (hit_c) { count++; }

            return count;
        }

        class Triangle : IEquatable<Triangle>
        {
            public Mesh.Vertex A, B, C;
            public int material_id;

            public Triangle(Triangle b)
            {
                this.material_id = b.material_id;
                this.A = new Mesh.Vertex(b.A);
                this.B = new Mesh.Vertex(b.B);
                this.C = new Mesh.Vertex(b.C);
            }

            public Triangle(int material_id, Mesh.Vertex A, Mesh.Vertex B, Mesh.Vertex C)
            {
                this.material_id = material_id;
                this.A = new Mesh.Vertex(A);
                this.B = new Mesh.Vertex(B);
                this.C = new Mesh.Vertex(C);
            }

            public bool Equals(Triangle other)
            {
                if (A.vertex.Equals(other.A.vertex) && B.vertex.Equals(other.B.vertex) && C.vertex.Equals(other.C.vertex)
                    && A.textcoords.Equals(other.A.textcoords) && B.textcoords.Equals(other.B.textcoords) && C.textcoords.Equals(other.C.textcoords)
                    && material_id == other.material_id)
                {
                    return true;
                }
                return false;
            }

        }

        private double ToRadian(double degree)
        {
            return (Math.PI * degree / 180.0);
        }

        bool IsValidTriangle(Vector3 A, Vector3 B, Vector3 C)
        {
            if (A.Equals(B) || B.Equals(C) || C.Equals(A)) return false;

            double radian = Math.Acos((double)Vector3.Dot(Vector3.Normalize(B - A), Vector3.Normalize(C - A)));
            if (radian < ToRadian(0.001)) return false;
            if (radian > ToRadian(179.999)) return false;

            return true;
        }

        const int SIZE = 10;
        List<Triangle>[,,] part;
        static Vector3 part_min, part_max;
        Vector3 step;

        void PointToID(Vector3 P, out int i, out int j, out int k)
        {
            i = (int)Math.Floor((double)((P.X - part_min.X) / step.X));
            j = (int)Math.Floor((double)((P.Y - part_min.Y) / step.Y));
            k = (int)Math.Floor((double)((P.Z - part_min.Z) / step.Z));
        }

        float Min(float a, float b)
        {
            if (a < b) return a;
            return b;
        }

        float Max(float a, float b)
        {
            if (a > b) return a;
            return b;
        }

        void PartAdd(Triangle triangle)
        {
            Vector3 min = new Vector3(+1000000000, +1000000000, +1000000000);
            Vector3 max = new Vector3(-1000000000, -1000000000, -1000000000);

            // min
            min.X = Min(min.X, triangle.A.vertex.X);
            min.X = Min(min.X, triangle.B.vertex.X);
            min.X = Min(min.X, triangle.C.vertex.X);

            min.Y = Min(min.Y, triangle.A.vertex.Y);
            min.Y = Min(min.Y, triangle.B.vertex.Y);
            min.Y = Min(min.Y, triangle.C.vertex.Y);

            min.Z = Min(min.Z, triangle.A.vertex.Z);
            min.Z = Min(min.Z, triangle.B.vertex.Z);
            min.Z = Min(min.Z, triangle.C.vertex.Z);

            // max
            max.X = Max(max.X, triangle.A.vertex.X);
            max.X = Max(max.X, triangle.B.vertex.X);
            max.X = Max(max.X, triangle.C.vertex.X);

            max.Y = Max(max.Y, triangle.A.vertex.Y);
            max.Y = Max(max.Y, triangle.B.vertex.Y);
            max.Y = Max(max.Y, triangle.C.vertex.Y);

            max.Z = Max(max.Z, triangle.A.vertex.Z);
            max.Z = Max(max.Z, triangle.B.vertex.Z);
            max.Z = Max(max.Z, triangle.C.vertex.Z);

            int min_i, min_j, min_k; PointToID(min, out min_i, out min_j, out min_k);
            int max_i, max_j, max_k; PointToID(max, out max_i, out max_j, out max_k);

            bool is_add = false;

            for (int i = min_i; i <= max_i; i++)
            {
                for (int j = min_j; j <= max_j; j++)
                {
                    for (int k = min_k; k <= max_k; k++)
                    {
                        if (part[i, j, k].Contains(triangle) == false)
                        {
                            part[i, j, k].Add(triangle);
                            is_add = true;
                        }
                    }
                }
            }

            if (is_add)
            {
                num_vertices += 3;
            }
        }

        void PartRemove(Triangle triangle)
        {
            Vector3 min = new Vector3(+1000000000, +1000000000, +1000000000);
            Vector3 max = new Vector3(-1000000000, -1000000000, -1000000000);

            // min
            min.X = Min(min.X, triangle.A.vertex.X);
            min.X = Min(min.X, triangle.B.vertex.X);
            min.X = Min(min.X, triangle.C.vertex.X);

            min.Y = Min(min.Y, triangle.A.vertex.Y);
            min.Y = Min(min.Y, triangle.B.vertex.Y);
            min.Y = Min(min.Y, triangle.C.vertex.Y);

            min.Z = Min(min.Z, triangle.A.vertex.Z);
            min.Z = Min(min.Z, triangle.B.vertex.Z);
            min.Z = Min(min.Z, triangle.C.vertex.Z);

            // max
            max.X = Max(max.X, triangle.A.vertex.X);
            max.X = Max(max.X, triangle.B.vertex.X);
            max.X = Max(max.X, triangle.C.vertex.X);

            max.Y = Max(max.Y, triangle.A.vertex.Y);
            max.Y = Max(max.Y, triangle.B.vertex.Y);
            max.Y = Max(max.Y, triangle.C.vertex.Y);

            max.Z = Max(max.Z, triangle.A.vertex.Z);
            max.Z = Max(max.Z, triangle.B.vertex.Z);
            max.Z = Max(max.Z, triangle.C.vertex.Z);

            int min_i, min_j, min_k; PointToID(min, out min_i, out min_j, out min_k);
            int max_i, max_j, max_k; PointToID(max, out max_i, out max_j, out max_k);

            bool is_remove = false;

            for (int i = min_i; i <= max_i; i++)
            {
                for (int j = min_j; j <= max_j; j++)
                {
                    for (int k = min_k; k <= max_k; k++)
                    {
                        while (part[i, j, k].Remove(triangle)) is_remove = true;
                    }
                }
            }

            if (is_remove)
            {
                num_vertices -= 3;
            }
        }

        public void Reduce(MainWindow window, int end_vertices, System.Windows.Forms.ProgressBar progress_bar, System.Windows.Forms.Label label)
        {
            progress_bar.Value = 0; progress_bar.Update();
            label.Text = "initializing"; label.Update();

            DateTime elapsed_datetime;
            DateTime current_datetime;
            current_datetime = elapsed_datetime = DateTime.Now;

            // partitioning triangles
            part = new List<Triangle>[SIZE + 1, SIZE + 1, SIZE + 1];
            for (int i = 0; i <= SIZE; i++)
            {
                for (int j = 0; j <= SIZE; j++)
                {
                    for (int k = 0; k <= SIZE; k++)
                    {
                        part[i, j, k] = new List<Triangle>();
                    }
                }
            }

            // min, max update
            mesh.min = new Vector3(+1000000000.0f, +1000000000.0f, +1000000000.0f);
            mesh.max = new Vector3(-1000000000.0f, -1000000000.0f, -1000000000.0f);
            foreach (Mesh.Material mat in mesh.materials)
            {
                foreach (Mesh.Vertex v in mat.vertices)
                {
                    // min
                    if (v.vertex.X < mesh.min.X) { mesh.min.X = v.vertex.X; }
                    if (v.vertex.Y < mesh.min.Y) { mesh.min.Y = v.vertex.Y; }
                    if (v.vertex.Z < mesh.min.Z) { mesh.min.Z = v.vertex.Z; }
                    // max
                    if (mesh.max.X < v.vertex.X) { mesh.max.X = v.vertex.X; }
                    if (mesh.max.Y < v.vertex.Y) { mesh.max.Y = v.vertex.Y; }
                    if (mesh.max.Z < v.vertex.Z) { mesh.max.Z = v.vertex.Z; }
                }
            }

            // start
            {
                part_min = mesh.min /*- new Vector3(0.001f, 0.001f, 0.001f)*/;
                part_max = mesh.max /*+ new Vector3(0.001f, 0.001f, 0.001f)*/;
                step = (part_max - part_min) / (float)SIZE;
            }

            num_vertices = 0;
            for (int i = 0; i < mesh.materials.Count; i++)
            {
                Mesh.Material material = mesh.materials[i];
            
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {
                    if (   material.vertices[v + 0] == null
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }
            
                    Triangle triangle = new Triangle(i, material.vertices[v + 0], material.vertices[v + 1], material.vertices[v + 2]);
                    PartAdd(triangle);
                }
            }

            // while, erase line
            System.Console.WriteLine(PartGetVertices(/*triangles*/));
            System.Console.WriteLine(end_vertices);

            if (PartGetVertices(/*triangles*/) < end_vertices)
            {
                end_vertices = PartGetVertices(/*triangles*/);
            }
            progress_bar.Maximum = PartGetVertices(/*triangles*/) - end_vertices;
            int elapsed_vertices = PartGetVertices(/*triangles*/);
            int current_vertices = PartGetVertices(/*triangles*/);

            Edge edge;
            //bad_edges = new List<Edge>();
            List<Triangle> buff = new List<Triangle>();
            bool hit_a, hit_b, hit_c;

            for (int i = PartGetVertices(/*triangles*/); i > end_vertices; i = PartGetVertices(/*triangles*/))
            {
                //System.Console.WriteLine("----------------------------------------------------------");
                bool is_ok;
                do
                {
                    //System.Console.WriteLine(".");
                    is_ok = false;
                    buff.Clear();
            
                    if (false == SearchMinEdge(/*triangles,*/ 1000, out edge))
                    {
                        System.Console.WriteLine("error min_edge");
                        goto reduce_stop;
                    }
            
                    // calc min, max
                    // get triangles by edge
                    Vector3 A = edge.a;
                    Vector3 B = edge.b;
                    
                    Vector3 min = new Vector3(+1000000000.0f, +1000000000.0f, +1000000000.0f);
                    Vector3 max = new Vector3(-1000000000.0f, -1000000000.0f, -1000000000.0f);
                    // min
                    // x
                    if (A.X < min.X) min.X = A.X;
                    if (B.X < min.X) min.X = B.X;
                    // y
                    if (A.Y < min.Y) min.Y = A.Y;
                    if (B.Y < min.Y) min.Y = B.Y;
                    // z
                    if (A.Z < min.Z) min.Z = A.Z;
                    if (B.Z < min.Z) min.Z = B.Z;
                    
                    // max
                    // x
                    if (A.X > max.X) max.X = A.X;
                    if (B.X > max.X) max.X = B.X;
                    // y
                    if (A.Y > max.Y) max.Y = A.Y;
                    if (B.Y > max.Y) max.Y = B.Y;
                    // z
                    if (A.Z > max.Z) max.Z = A.Z;
                    if (B.Z > max.Z) max.Z = B.Z;
                    
                    int min_i, min_j, min_k; PointToID(min, out min_i, out min_j, out min_k);
                    int max_i, max_j, max_k; PointToID(max, out max_i, out max_j, out max_k);
                    
                    for (int ii = min_i; ii <= max_i; ii++)
                    {
                        for (int jj = min_j; jj <= max_j; jj++)
                        {
                            for (int kk = min_k; kk <= max_k; kk++)
                            {
                                foreach (Triangle triangle in part[ii, jj, kk])
                                {
                                    int count = NumEdgePointsInTriangle(edge, triangle.A.vertex, triangle.B.vertex, triangle.C.vertex, out hit_a, out hit_b, out hit_c);
                    
                                    if (count > 0)
                                    {
                                        buff.Add(new Triangle(triangle));
                                    }
                                }
                            }
                        }
                    }
            
                    if (buff.Count == 0)
                    {
                        //if (false == bad_edges.Contains(edge)) bad_edges.Add(edge);
                    }
                    else
                    {
                        is_ok = true;
                    }
                }
                while (!is_ok);
                //System.Console.WriteLine("edge ok: " + buff.Count);
            
                // calc
                for (int w = 0; w < buff.Count; w++)
                {
                    Triangle triangle = buff[w];
            
                    PartRemove(triangle);
            
                    //while (triangles.Remove(triangle)) ;
                }
            
                // modify
                Vector3 new_vertex = (edge.a + edge.b) * 0.5f;
                foreach (Triangle triangle in buff)
                {
                    int count = NumEdgePointsInTriangle(edge, triangle.A.vertex, triangle.B.vertex, triangle.C.vertex, out hit_a, out hit_b, out hit_c);
            
                    if (2 == count)
                    {
                        if (hit_a && hit_b)
                        {
                            Vector2 new_textcoord = (triangle.A.textcoords + triangle.B.textcoords) * 0.5f;
            
                            triangle.A.vertex = new_vertex;
                            triangle.A.textcoords = new_textcoord;
            
                            triangle.B.vertex = new_vertex;
                            triangle.B.textcoords = new_textcoord;
                        }
                        else if (hit_b && hit_c)
                        {
                            Vector2 new_textcoord = (triangle.B.textcoords + triangle.C.textcoords) * 0.5f;
            
                            triangle.B.vertex = new_vertex;
                            triangle.B.textcoords = new_textcoord;
            
                            triangle.C.vertex = new_vertex;
                            triangle.C.textcoords = new_textcoord;
                        }
                        else if (hit_c && hit_a)
                        {
                            Vector2 new_textcoord = (triangle.C.textcoords + triangle.A.textcoords) * 0.5f;
            
                            triangle.C.vertex = new_vertex;
                            triangle.C.textcoords = new_textcoord;
            
                            triangle.A.vertex = new_vertex;
                            triangle.A.textcoords = new_textcoord;
                        }
                        else
                        {
                            //System.Console.WriteLine("error 2");
                        }
                    }
                    else if (1 == count)
                    {
                        if (hit_a) { triangle.A.vertex = new_vertex; }
                        else if (hit_b) { triangle.B.vertex = new_vertex; }
                        else if (hit_c) { triangle.C.vertex = new_vertex; }
                        else { /*System.Console.WriteLine("error 1");*/ }
                    }
                    else
                    {
                        //System.Console.WriteLine("error " + count);
                        //if (false == bad_edges.Contains(edge)) bad_edges.Add(edge);
                    }
                }
            
                // add
                foreach (Triangle triangle in buff)
                {
                    if (IsValidTriangle(triangle.A.vertex, triangle.B.vertex, triangle.C.vertex))
                    {
                        //triangles.Add(triangle);
                        PartAdd(triangle);
                    }
                }
            
                int value = (int)((float)progress_bar.Maximum * (1.0f - ((float)(i - end_vertices) / (float)progress_bar.Maximum)));
                if (value > progress_bar.Maximum) value = progress_bar.Maximum;
                float percent = (((float)value / (float)progress_bar.Maximum) * 100.0f);

                current_datetime = DateTime.Now;
                TimeSpan diff = current_datetime - elapsed_datetime;
                float dt = (float)diff.TotalMilliseconds / 1000.0f;
                if (dt >= 0.05f)
                {
                    elapsed_datetime = current_datetime;

                    progress_bar.Value = value;
                    label.Text = percent.ToString("###0.00") + " %";
                    progress_bar.Update();
                    label.Update();

                    Application.DoEvents();
                }

                elapsed_vertices = current_vertices;
                current_vertices = PartGetVertices(/*triangles*/);
                if (elapsed_vertices == current_vertices)
                {
                    //if (false == bad_edges.Contains(edge)) bad_edges.Add(edge);                   
                }
            
                // release
                buff.Clear();
            }
      
            reduce_stop:
      
            // triangles to mesh
            // clear mesh
            foreach (Mesh.Material material in mesh.materials)
            {
                foreach (Mesh.Vertex vertex in material.vertices)
                {
                    if (vertex == null) { continue; }
      
                    vertex.matrices.Clear();
                }
                material.vertices.Clear();
            }

            // copy to mesh
            List<Triangle> triangles = new List<Triangle>();
            List<Triangle> triangles_buff = new List<Triangle>();
            for (int i = 0; i <= SIZE; i++)
            {
                for (int j = 0; j <= SIZE; j++)
                {
                    for (int k = 0; k <= SIZE; k++)
                    {
                        foreach (Triangle tri in part[i, j, k])
                        {
                            if (false == triangles_buff.Contains(tri))
                            {
                                triangles_buff.Add(tri);

                                if (triangles_buff.Count > 10000)
                                {
                                    triangles.AddRange(triangles_buff);
                                    triangles_buff.Clear();
                                }
                            }
                        }
                    }
                }
            }
            triangles.AddRange(triangles_buff);
            triangles_buff.Clear();

            foreach (Triangle tri in triangles)
            {
                mesh.materials[tri.material_id].vertices.Add(tri.A);
                mesh.materials[tri.material_id].vertices.Add(tri.B);
                mesh.materials[tri.material_id].vertices.Add(tri.C);
            }

            // min, max update
            mesh.min = new Vector3(+1000000000.0f, +1000000000.0f, +1000000000.0f);
            mesh.max = new Vector3(-1000000000.0f, -1000000000.0f, -1000000000.0f);
            foreach (Mesh.Material mat in mesh.materials)
            {
                foreach (Mesh.Vertex v in mat.vertices)
                {
                    // min
                    if (v.vertex.X < mesh.min.X) { mesh.min.X = v.vertex.X; }
                    if (v.vertex.Y < mesh.min.Y) { mesh.min.Y = v.vertex.Y; }
                    if (v.vertex.Z < mesh.min.Z) { mesh.min.Z = v.vertex.Z; }
                    // max
                    if (mesh.max.X < v.vertex.X) { mesh.max.X = v.vertex.X; }
                    if (mesh.max.Y < v.vertex.Y) { mesh.max.Y = v.vertex.Y; }
                    if (mesh.max.Z < v.vertex.Z) { mesh.max.Z = v.vertex.Z; }
                }
            }
      
            triangles.Clear();
            triangles = null;
            triangles_buff = null;
      
            System.GC.Collect();
        }
    }
}
