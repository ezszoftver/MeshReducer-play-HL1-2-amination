using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Numerics;

namespace MeshReducer
{
    public class Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Triangle()
        {
            A = new Vector3();
            B = new Vector3();
            C = new Vector3();
        }

        public Triangle(Triangle b)
        {
            this.A = new Vector3(b.A.X, b.A.Y, b.A.Z);
            this.B = new Vector3(b.B.X, b.B.Y, b.B.Z);
            this.C = new Vector3(b.C.X, b.C.Y, b.C.Z);
        }

        public Triangle(Vector3 A, Vector3 B, Vector3 C)
        {
            this.A = new Vector3(A.X, A.Y, A.Z);
            this.B = new Vector3(B.X, B.Y, B.Z);
            this.C = new Vector3(C.X, C.Y, C.Z);
        }

        public Vector3 GetNormal()
        {
            Vector3 N = Vector3.Normalize(Vector3.Cross(B - A, C - A));
            return N;
        }

        private static bool IsEqual(float a, float b)
        {
            if (Math.Abs(b - a) < 0.001f)
            {
                return true;
            }
            return false;
        }

        public static bool IsEqual(Vector3 A, Vector3 B)
        {
            if (IsEqual(A.X, B.X) && IsEqual(A.Y, B.Y) && IsEqual(A.Z, B.Z))
            {
                return true;
            }
            return false;
        }

        public bool Equals(Triangle other)
        {
            if (IsEqual(A, other.A) && IsEqual(B, other.B) && IsEqual(C, other.C))
            {
                return true;
            }
            return false;
        }
    }

    class MeshToFile
    {
        private List<Triangle>[,,] part;

        private Vector3 start;
        private Vector3 step;

        public MeshToFile()
        {
            part = null;
            start = new Vector3(0, 0, 0);
            step = new Vector3(0, 0, 0);
        }

        bool IsEqual(float a, float b)
        {
            if (Math.Abs(b - a) < 0.001f)
            {
                return true;
            }
            return false;
        }

        bool IsEqual(Vector3 A, Vector3 B)
        {
            if (IsEqual(A.X, B.X) && IsEqual(A.Y, B.Y) && IsEqual(A.Z, B.Z))
            {
                return true;
            }
            return false;
        }

        public void PointToID(Vector3 P, out int i, out int j, out int k)
        {
            i = (int)Math.Floor((P.X - start.X) / step.X);
            j = (int)Math.Floor((P.Y - start.Y) / step.Y);
            k = (int)Math.Floor((P.Z - start.Z) / step.Z);

            if (i < 0) i = 0;
            if (j < 0) j = 0;
            if (k < 0) k = 0;

            if (i >= SIZE) i = SIZE - 1;
            if (j >= SIZE) j = SIZE - 1;
            if (k >= SIZE) k = SIZE - 1;
        }

        public enum SaveFileType { OBJ = 1, HL1SMD = 2, HL2SMD = 3 };
        public bool SaveToFile(Mesh mesh, System.Windows.Forms.ProgressBar progress_bar, System.Windows.Forms.Label label, SaveFileType extension, string path, string filename)
        {
            switch (extension)
            {
                case (SaveFileType.OBJ):
                    {
                        return SaveToOBJ(mesh, progress_bar, label, path, filename);
                    }
                case (SaveFileType.HL1SMD):
                    {
                        return SaveToHL1SMD(mesh, progress_bar, label, path, filename);
                    }
                case (SaveFileType.HL2SMD):
                    {
                        return SaveToHL2SMD(mesh, progress_bar, label, path, filename);
                    }
            }

            return false;
        }

        public const int SIZE = 100;

        void MeshPartitioning(Mesh mesh)
        {
            // init
            part = new List<Triangle>[SIZE, SIZE, SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    for (int k = 0; k < SIZE; k++)
                    {
                        part[i, j, k] = new List<Triangle>();
                    }
                }
            }
            // start
            start = mesh.min;
            // step
            step.X = (mesh.max.X - mesh.min.X) / (float)SIZE;
            step.Y = (mesh.max.Y - mesh.min.Y) / (float)SIZE;
            step.Z = (mesh.max.Z - mesh.min.Z) / (float)SIZE;

            Vector3 min;
            Vector3 max;
            Triangle tri;

            foreach (Mesh.Material material in mesh.materials)
            {
                for (int v = 0; v < material.vertices.Count(); v += 3)
                {
                    if (   material.vertices[v + 0] == null 
                        || material.vertices[v + 1] == null
                        || material.vertices[v + 2] == null)
                    {
                        continue;
                    }

                    Vector3 A = material.vertices[v + 0].vertex;
                    Vector3 B = material.vertices[v + 1].vertex;
                    Vector3 C = material.vertices[v + 2].vertex;
                    
                    min = new Vector3( 1000000.0f, 1000000.0f, 1000000.0f);
                    max = new Vector3(-1000000.0f,-1000000.0f,-1000000.0f);
                    // min
                    // x
                    if (A.X < min.X) min.X = A.X;
                    if (B.X < min.X) min.X = B.X;
                    if (C.X < min.X) min.X = C.X;
                    // y
                    if (A.Y < min.Y) min.Y = A.Y;
                    if (B.Y < min.Y) min.Y = B.Y;
                    if (C.Y < min.Y) min.Y = C.Y;
                    // z
                    if (A.Z < min.Z) min.Z = A.Z;
                    if (B.Z < min.Z) min.Z = B.Z;
                    if (C.Z < min.Z) min.Z = C.Z;

                    // max
                    // x
                    if (A.X > max.X) max.X = A.X;
                    if (B.X > max.X) max.X = B.X;
                    if (C.X > max.X) max.X = C.X;
                    // y
                    if (A.Y > max.Y) max.Y = A.Y;
                    if (B.Y > max.Y) max.Y = B.Y;
                    if (C.Y > max.Y) max.Y = C.Y;
                    // z
                    if (A.Z > max.Z) max.Z = A.Z;
                    if (B.Z > max.Z) max.Z = B.Z;
                    if (C.Z > max.Z) max.Z = C.Z;

                    min -= new Vector3(0.001f, 0.001f, 0.001f);
                    max += new Vector3(0.001f, 0.001f, 0.001f);

                    int min_i, min_j, min_k; PointToID(min, out min_i, out min_j, out min_k);
                    int max_i, max_j, max_k; PointToID(max, out max_i, out max_j, out max_k);

                    for (int i = min_i; i <= max_i; i++)
                    {
                        for (int j = min_j; j <= max_j; j++)
                        {
                            for (int k = min_k; k <= max_k; k++)
                            {
                                tri = new Triangle(A, B, C);

                                if (false == part[i, j, k].Contains(tri))
                                {
                                    part[i, j, k].Add(tri);
                                }
                            }
                        }
                    }
                }
            }
        }

        float RadianToDegree(float radian)
        {
            return (radian * (180.0f / (float)Math.PI));
        }

        float GetAngle(Vector3 a, Vector3 b)
        {
            return RadianToDegree((float)Math.Acos(Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b))));
        }

        Vector3 GetNormal(Triangle triangle, Vector3 vertex)
        {
            int i, j, k;
            PointToID(vertex, out i, out j, out k);

            Vector3 global_normal = triangle.GetNormal();
            Vector3 triangle_normal = triangle.GetNormal();

            foreach (Triangle tri in part[i, j, k])
            {
                Vector3 local_normal = tri.GetNormal();

                if (GetAngle(triangle_normal, local_normal) < 45.0f)
                {
                    global_normal += local_normal;
                    global_normal = Vector3.Normalize(global_normal);
                }
            }

            return global_normal;
        }

        bool SaveToOBJ(Mesh mesh, System.Windows.Forms.ProgressBar progress_bar, System.Windows.Forms.Label label, string directory, string filename)
        {
            MeshPartitioning(mesh);

            System.Console.WriteLine("directory: " + directory);
            System.Console.WriteLine("filename: " + filename);

            StreamWriter file = File.CreateText(directory + "/" + filename + ".obj");

            file.WriteLine("# EZSZOFTVER - Mesh Reducer v1.0 (2017)");
            file.WriteLine("# http://ezszoftver.atw.hu/");
            file.WriteLine("# Num Vertices: " + mesh.GetVerticesCount());
            file.WriteLine("");
            if (mesh.mtllib.Length > 0)
            {
                file.WriteLine("mtllib " + mesh.mtllib);
            }
            else
            {
                StreamWriter mtl = File.CreateText(directory + "/" + filename + ".mtl");

                for (int i = 0; i < mesh.materials.Count; i++)
                {
                    Mesh.Material material = mesh.materials[i];

                    mtl.WriteLine("newmtl material_" + i);
                    mtl.WriteLine("map_Kd " + material.texture_name);
                    mtl.WriteLine();
                }

                mtl.Close();

                file.WriteLine("mtllib " + filename + ".mtl");
            }

            file.WriteLine("");

            int face = 1;

            for (int m = 0; m < mesh.materials.Count; m++)
            {
                Mesh.Material material = mesh.materials[m];

                file.WriteLine("usemtl material_" + m);

                for (int i = 0; i < material.vertices.Count; i += 3)
                {
                    if (   material.vertices[i + 0] == null
                        || material.vertices[i + 1] == null
                        || material.vertices[i + 2] == null)
                    {
                        continue;
                    }

                    Vector3 A = material.vertices[i + 0].vertex;
                    Vector3 B = material.vertices[i + 1].vertex;
                    Vector3 C = material.vertices[i + 2].vertex;

                    Vector2 vat = material.vertices[i + 0].textcoords;
                    Vector2 vbt = material.vertices[i + 1].textcoords;
                    Vector2 vct = material.vertices[i + 2].textcoords;

                    Vector3 an = GetNormal(new Triangle(A, B, C), A);
                    Vector3 bn = GetNormal(new Triangle(A, B, C), B);
                    Vector3 cn = GetNormal(new Triangle(A, B, C), C);

                    if (an.X.ToString() == "NaN" || an.Y.ToString() == "NaN" || an.Z.ToString() == "NaN") { /*an = new Vector3(1, 0, 0);*/continue; }
                    if (bn.X.ToString() == "NaN" || bn.Y.ToString() == "NaN" || bn.Z.ToString() == "NaN") { /*bn = new Vector3(1, 0, 0);*/continue; }
                    if (cn.X.ToString() == "NaN" || cn.Y.ToString() == "NaN" || cn.Z.ToString() == "NaN") { /*cn = new Vector3(1, 0, 0);*/continue; }

                    // A
                    file.WriteLine("v " + A.X + " " + A.Y + " " + A.Z);
                    file.WriteLine("vt " + vat.X + " " + vat.Y);
                    file.WriteLine("vn " + an.X + " " + an.Y + " " + an.Z);
                    
                    // B
                    file.WriteLine("v " + B.X + " " + B.Y + " " + B.Z);
                    file.WriteLine("vt " + vbt.X + " " + vbt.Y);
                    file.WriteLine("vn " + bn.X + " " + bn.Y + " " + bn.Z);
                    
                    // C
                    file.WriteLine("v " + C.X + " " + C.Y + " " + C.Z);
                    file.WriteLine("vt " + vct.X + " " + vct.Y);
                    file.WriteLine("vn " + cn.X + " " + cn.Y + " " + cn.Z);

                    // face (triangle)
                    file.WriteLine("f " + (face + 0) + "/" + (face + 0) + "/" + (face + 0) + " "
                                        + (face + 1) + "/" + (face + 1) + "/" + (face + 1) + " "
                                        + (face + 2) + "/" + (face + 2) + "/" + (face + 2));

                    face += 3;
                }

                file.WriteLine("");
            }

            file.Close();
            return true;
        }

        bool SaveToHL1SMD(Mesh mesh, System.Windows.Forms.ProgressBar progress_bar, System.Windows.Forms.Label label, string path, string filename)
        {
            MeshPartitioning(mesh);

            ;

            return true;
        }

        bool SaveToHL2SMD(Mesh mesh, System.Windows.Forms.ProgressBar progress_bar, System.Windows.Forms.Label label, string path, string filename)
        {
            MeshPartitioning(mesh);

            ;

            return true;
        }

        public void Release()
        {
            if (part != null) {
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        for (int k = 0; k < SIZE; k++)
                        {
                            part[i, j, k].Clear();
                            part[i, j, k] = null;
                        }
                    }
                }
                part = null;
            }
            start = new Vector3(0, 0, 0);
            step = new Vector3(0, 0, 0);
        }
    }
}
