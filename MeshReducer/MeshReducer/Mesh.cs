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

        public Mesh()
        {
            is_loaded = false;
            transforms = new List<Matrix4x4>();
            inverse_transforms_reference = new List<Matrix4x4>();
            materials = new List<Material>();
            min = new Vector3(0, 0, 0);
            max = new Vector3(0, 0, 0);
        }

        public void Draw(bool is_animated)
        {
            if (is_animated) // animated model
            {
                foreach (Material material in materials)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (Vertex vertex in material.vertices)
                    {
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
                        Vector3 v = vertex.vertex;
                        Vector2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.X, t.Y);
                        Gl.glVertex3f(v.X, v.Y, v.Z);
                    }
                    Gl.glEnd();
                }
            }
        }

        public void Release()
        {
            foreach (Material material in materials)
            {
                material.texture.Release();
                material.texture = null;

                foreach (Vertex vertex in material.vertices)
                {
                    vertex.matrices.Clear();
                    vertex.matrices = null;
                }

                material.vertices.Clear();
                material.vertices = null;
            }

            transforms.Clear();
            materials.Clear();
        }
    }
}
