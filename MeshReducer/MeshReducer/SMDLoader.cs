using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Tao.OpenGl;
using GlmNet;

using MeshReducer.Texture;

namespace MeshReducer.SMDLoader
{
    class SMDLoader
    {
        // egy csomópont
        class Node
        {
		public Node(string name, int parent_id)
            {
                this.parent_id = parent_id;
                this.name = name;
            }

            string name;
            int parent_id;
        };
        // csomópontok
        List<Node> nodes;

        // egy csont
        class Bone
        {
		    public Bone(vec3 translate, vec3 rotate)
            {
                this.translate = translate;
                this.rotate = rotate;

                this.is_updated = false;
                this.transform = new mat4(1.0f);
                this.transform_inverse = new mat4(1.0f);
            }

            vec3 translate;
            vec3 rotate;
            // matrix-ok
            bool is_updated;
            mat4 transform;
            mat4 transform_inverse;
        };

        // egy csontváz
        class Skeleton
        {
            public Skeleton()
            {
                bones = new List<Bone>();
            }

		    public List<Bone> bones;
        };

        // egy animáció
        class Animation
        {
    		public string name;
            float fps; // 1 sec alatt, hány skeleton játszódik le
            List<Skeleton> times;

            public Animation()
            {
                times = new List<Skeleton>();
                name = "";
                fps = 0;
            }
        };
        // animációk
        List<Animation> animations;

        Skeleton reference_skeleton;

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
            public Vertex(vec3 vertex, vec2 textcoords)
            {
                this.vertex = vertex;
                this.textcoords = textcoords;
            }

            public void AddMatrix(int matrix_id, float weight)
            {
                matrices.Add(new MatrixIdAndWeight(matrix_id, weight));
            }

            public vec3 vertex;
            public vec2 textcoords;
            public List<MatrixIdAndWeight> matrices = new List<MatrixIdAndWeight>();
        }

        public class Material
        {
            public string texture_name;
            public Texture.Texture texture;

            public List<Vertex> vertices;

            public Material(string texture_name)
            {
                vertices = new List<Vertex>();
                this.texture_name = texture_name;
                texture = new Texture.Texture();
            }
        }
        public List<Material> materials;

        private Dictionary<string, int> material_to_id;
        public vec3 min, max;

        public SMDLoader()
        {
            nodes = new List<Node>();
            animations = new List<Animation>();
            materials = new List<Material>();
            material_to_id = new Dictionary<string, int>();
            reference_skeleton = new Skeleton();
            min = new vec3(0, 0, 0);
            max = new vec3(0, 0, 0);
        }

        public bool LoadReference(string directory, string filename)
        {
            string[] lines = File.ReadAllText(directory + @"\" + filename).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int mat_id = -1;

            bool is_nodes = false;
            bool is_skeleton_header = false;
            bool is_skeleton = false;
            bool is_triangles_texturename = false;
            bool is_triangles_v = false;
            int is_triangles_vrepeat = 0;

            foreach (string line in lines)
            {
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words[0] == "nodes") { is_nodes = true; continue; }
                if (words[0] == "skeleton") { is_skeleton_header = true; continue; }
                if (words[0] == "triangles") { is_triangles_texturename = true; continue; }
                
                if (is_nodes)
                {
                    if (words[0] == "end") { is_nodes = false; continue; }

                    nodes.Add(new Node(words[1], int.Parse(words[2])));
                }

                if (is_skeleton_header)
                {
                    is_skeleton_header = false;
                    is_skeleton = true;
                    continue;
                }

                if (is_skeleton)
                {
                    if (words[0] == "end") { is_skeleton = false; continue; }

                    reference_skeleton.bones.Add(new Bone(new vec3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])), new vec3(float.Parse(words[4]), float.Parse(words[5]), float.Parse(words[6]))));
                }

                if (is_triangles_texturename)
                {
                    if (words[0] == "end") { is_triangles_texturename = false; continue; }

                    string texture_name = words[0];

                    if (!material_to_id.ContainsKey(texture_name)) {
                        mat_id++;
                        material_to_id.Add(texture_name, mat_id);
                        materials.Add(new Material(texture_name));
                    }

                    is_triangles_texturename = false;
                    is_triangles_v = true;
                    is_triangles_vrepeat = 0;
                    continue;
                }

                if (is_triangles_v)
                {
                    if (words.Count() == 9) // HL1
                    {
                        // matrix
                        int matrix_id = int.Parse(words[0]);
                        float weight = 1.0f;
                        // vertex
                        vec3 v = new vec3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                        // textcoords
                        vec2 t = new vec2(float.Parse(words[7]), float.Parse(words[8]));

                        Vertex vertex = new Vertex(v, t);

                        // one matrix
                        vertex.matrices.Add(new MatrixIdAndWeight(matrix_id, weight));

                        // add
                        materials[mat_id].vertices.Add(vertex);
                    }
                    else // HL2
                    {
                        // vertex
                        vec3 v = new vec3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                        // textcoords
                        vec2 t = new vec2(float.Parse(words[7]), float.Parse(words[8]));

                        Vertex vertex = new Vertex(v, t);

                        // many matrix
                        int n = int.Parse(words[9]);
                        int id = 10;
                        for (int i = 0; i < n; i++)
                        {
                            // matrix
                            int matrix_id = int.Parse(words[id++]);
                            float weight = float.Parse(words[id++]);

                            // add
                            vertex.matrices.Add(new MatrixIdAndWeight(matrix_id, weight));
                        }

                        // add
                        materials[mat_id].vertices.Add(vertex);
                    }

                    is_triangles_vrepeat++;
                    if (is_triangles_vrepeat == 3)
                    {
                        is_triangles_v = false;
                        is_triangles_texturename = true;
                        continue;
                    }
                    
                    continue;
                }
            }

            // min-max
            min = new vec3(+1000000.0f, +1000000.0f, +1000000.0f);
            max = new vec3(-1000000.0f, -1000000.0f, -1000000.0f);
            foreach (Material material in materials)
            {
                foreach (Vertex vertex in material.vertices)
                {
                    vec3 v = vertex.vertex;

                    // min
                    if (v.x < min.x) { min.x = v.x; }
                    if (v.y < min.y) { min.y = v.y; }
                    if (v.z < min.z) { min.z = v.z; }
                    // max
                    if (max.x < v.x) { max.x = v.x; }
                    if (max.y < v.y) { max.y = v.y; }
                    if (max.z < v.z) { max.z = v.z; }
                }
            }

            return true;
        }

        public void Release()
        {
            // nodes
            nodes.Clear();

            ;
        }


    }
}
