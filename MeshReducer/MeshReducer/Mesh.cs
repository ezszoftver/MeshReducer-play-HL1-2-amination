using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using Tao.OpenGl;

namespace MeshReducer
{
    class Mesh
    {
        public class MatrixIdAndWeight
        {
            public MatrixIdAndWeight(int matrix_id, float weight)
            {
                this.weight = weight;
                this.matrix_id = matrix_id;
            }

            public int matrix_id;
            public float weight;
        }

        public class Vertex
        {
            public Vertex(Vector3 vertex, Vector2 textcoords)
            {
                this.vertex = vertex;
                this.textcoords = textcoords;
                this.matrices = new List<MatrixIdAndWeight>();
            }

            public Vertex(Vertex b)
            {
                vertex = new Vector3(b.vertex.X, b.vertex.Y, b.vertex.Z);
                textcoords = new Vector2(b.textcoords.X, b.textcoords.Y);
                matrices = new List<MatrixIdAndWeight>();
                foreach (MatrixIdAndWeight b_matrix in b.matrices)
                {
                    MatrixIdAndWeight matrix = new MatrixIdAndWeight(b_matrix.matrix_id, b_matrix.weight);
                    matrices.Add(matrix);
                }
            }

            public void AddMatrix(int matrix_id, float weight)
            {
                matrices.Add(new MatrixIdAndWeight(matrix_id, weight));
            }

            public Vector3 vertex;
            public Vector2 textcoords;
            public List<MatrixIdAndWeight> matrices;
        }

        public class Material
        {
            public string texture_name;
            public Texture texture;

            public List<Vertex> vertices;

            public Material(string texture_name)
            {
                vertices = new List<Vertex>();
                this.texture_name = texture_name;
                texture = new Texture();
            }
        }

        public List<Matrix4x4> inverse_transforms_reference;
        public List<Matrix4x4> transforms;
        public List<Material>  materials;
        public Vector3 min, max;
        public bool is_loaded;

        public string mtllib;
        public bool is_hl1;
        public bool is_hl2;

        public Mesh()
        {
            is_loaded = false;
            transforms = new List<Matrix4x4>();
            inverse_transforms_reference = new List<Matrix4x4>();
            materials = new List<Material>();
            min = new Vector3(0, 0, 0);
            max = new Vector3(0, 0, 0);
            mtllib = "";
            is_hl1 = false;
            is_hl2 = true;
        }

        public Mesh(Mesh b)
        {
            is_loaded = b.is_loaded;
            mtllib = b.mtllib;

            transforms = new List<Matrix4x4>();
            foreach (Matrix4x4 b_m in b.transforms)
            {
                Matrix4x4 m = new Matrix4x4(b_m.M11, b_m.M12, b_m.M13, b_m.M14,
                                            b_m.M21, b_m.M22, b_m.M23, b_m.M24,
                                            b_m.M31, b_m.M32, b_m.M33, b_m.M34,
                                            b_m.M41, b_m.M42, b_m.M43, b_m.M44);
                transforms.Add(m);
            }

            inverse_transforms_reference = new List<Matrix4x4>();
            foreach (Matrix4x4 b_m in b.inverse_transforms_reference)
            {
                Matrix4x4 m = new Matrix4x4(b_m.M11, b_m.M12, b_m.M13, b_m.M14,
                                            b_m.M21, b_m.M22, b_m.M23, b_m.M24,
                                            b_m.M31, b_m.M32, b_m.M33, b_m.M34,
                                            b_m.M41, b_m.M42, b_m.M43, b_m.M44);
                inverse_transforms_reference.Add(m);
            }

            materials = new List<Material>();
            min = new Vector3(+1000000.0f, +1000000.0f, +1000000.0f);
            max = new Vector3(-1000000.0f, -1000000.0f, -1000000.0f);

            foreach (Material b_mat in b.materials)
            {
                Material mat = new Material(b_mat.texture_name);
                mat.texture = b_mat.texture;

                foreach (Vertex b_v in b_mat.vertices)
                {
                    Vertex v = new Vertex(b_v);

                    // min
                    if (v.vertex.X < min.X) { min.X = v.vertex.X; }
                    if (v.vertex.Y < min.Y) { min.Y = v.vertex.Y; }
                    if (v.vertex.Z < min.Z) { min.Z = v.vertex.Z; }
                    // max
                    if (max.X < v.vertex.X) { max.X = v.vertex.X; }
                    if (max.Y < v.vertex.Y) { max.Y = v.vertex.Y; }
                    if (max.Z < v.vertex.Z) { max.Z = v.vertex.Z; }

                    mat.vertices.Add(v);
                }

                materials.Add(mat);
            }
        }

        public int GetVerticesCount()
        {
            int count = 0;

            foreach (Material material in materials)
            {
                foreach (Vertex vertex in material.vertices)
                {
                    if (vertex == null) continue;
                    count++;
                }
            }

            return count;
        }

        public void Draw(bool is_animated)
        {
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            if (is_animated) // animated model
            {
                foreach (Material material in materials)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (Vertex vertex in material.vertices)
                    {
                        if (vertex == null) { continue; }

                        // transform
                        Matrix4x4 transform = Matrix4x4.Multiply(inverse_transforms_reference[vertex.matrices[0].matrix_id], transforms[vertex.matrices[0].matrix_id]) * vertex.matrices[0].weight;
                        for (int i = 1; i < vertex.matrices.Count; i++)
                        {
                            transform += Matrix4x4.Multiply(inverse_transforms_reference[vertex.matrices[i].matrix_id], transforms[vertex.matrices[i].matrix_id]) * vertex.matrices[i].weight;
                        }
                        
                        Vector4 v = Vector4.Transform(new Vector4(vertex.vertex, 1), transform);
                        Vector2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.X, t.Y);
                        Gl.glVertex3f(v.X, v.Y, v.Z);
                    }
                    Gl.glEnd();
                }
            }
            else // static model
            {
                foreach (Material material in materials)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (Vertex vertex in material.vertices)
                    {
                        if (vertex == null) { continue; }

                        Vector3 v = vertex.vertex;
                        Vector2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.X, t.Y);
                        Gl.glVertex3f(v.X, v.Y, v.Z);
                    }
                    Gl.glEnd();
                }
            }
        }

        public void DrawWireFrame(bool is_animated)
        {
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            if (is_animated) // animated model
            {
                foreach (Material material in materials)
                {
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    Gl.glColor3f(1, 1, 1);
                    foreach (Vertex vertex in material.vertices)
                    {
                        if (vertex == null) { continue; }

                        // transform
                        Matrix4x4 transform = Matrix4x4.Multiply(inverse_transforms_reference[vertex.matrices[0].matrix_id], transforms[vertex.matrices[0].matrix_id]) * vertex.matrices[0].weight;
                        for (int i = 1; i < vertex.matrices.Count; i++)
                        {
                            transform += Matrix4x4.Multiply(inverse_transforms_reference[vertex.matrices[i].matrix_id], transforms[vertex.matrices[i].matrix_id]) * vertex.matrices[i].weight;
                        }

                        Vector4 v = Vector4.Transform(new Vector4(vertex.vertex, 1), transform);
                        Vector2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.X, t.Y);
                        Gl.glVertex3f(v.X, v.Y, v.Z);
                    }
                    Gl.glEnd();
                }
            }
            else // static model
            {
                foreach (Material material in materials)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (Vertex vertex in material.vertices)
                    {
                        if (vertex == null) { continue; }

                        Vector3 v = vertex.vertex;
                        Vector2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.X, t.Y);
                        Gl.glVertex3f(v.X, v.Y, v.Z);
                    }
                    Gl.glEnd();
                }
            }
        }

        public void Release(bool is_delete_textures)
        {
            foreach (Material material in materials)
            {
                if (is_delete_textures)
                {
                    material.texture.Release();
                    material.texture = null;
                }
                
                foreach (Vertex vertex in material.vertices)
                {
                    if (vertex == null) { continue; }

                    vertex.matrices.Clear();
                    vertex.matrices = null;
                }

                material.vertices.Clear();
                material.vertices = null;
            }

            transforms.Clear();
            transforms = null;
            inverse_transforms_reference.Clear();
            inverse_transforms_reference = null;

            if (is_delete_textures) { materials.Clear(); }
        }
    }
}
