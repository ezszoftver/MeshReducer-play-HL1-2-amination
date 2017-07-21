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
        public class Node
        {
		public Node(string name, int parent_id)
            {
                this.parent_id = parent_id;
                this.name = name;
            }

            public string name;
            public int parent_id;
        };
        // csomópontok
        List<Node> nodes;

        // egy csont
        public class Bone
        {
		    public Bone(vec3 translate, vec3 rotate)
            {
                this.translate = translate;
                this.rotate = rotate;
            }

            public vec3 translate;
            public vec3 rotate;
        };

        // egy csontváz
        public class Skeleton
        {
            public Skeleton()
            {
                bones = new List<Bone>();
            }

		    public List<Bone> bones;
        };

        // egy animáció
        public class Animation
        {
    		public string name;
            public float fps; // 1 sec alatt, hány skeleton játszódik le
            public List<Skeleton> times;

            public Animation()
            {
                times = new List<Skeleton>();
                name = "";
                fps = 0;
            }
        };
        // animációk
        List<Animation> animations;

        public Skeleton reference_skeleton;
        public mat4[] transform_inverse;
        Skeleton current_skeleton;
        public mat4[] transform;

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
            current_skeleton = new Skeleton();
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
                        vertex.AddMatrix(matrix_id, weight);

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
                            vertex.AddMatrix(matrix_id, weight);
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

            // init current skeleton
            for (int i = 0; i < reference_skeleton.bones.Count(); i++)
            {
                current_skeleton.bones.Add(new Bone(new vec3(0, 0, 0), new vec3(0, 0, 0)));
            }

            // calc matrices
            transform = new mat4[reference_skeleton.bones.Count()];
            transform_inverse = new mat4[reference_skeleton.bones.Count()];
            for (int i = 0; i < reference_skeleton.bones.Count(); i++)
            {
                transform_inverse[i] = glm.inverse(GetReferenceMatrix(i));
            }

            return true;
        }

        public bool AddAnimation(string directory, string filename, string anim_name, float fps)
        {
            string[] lines = File.ReadAllText(directory + @"\" + filename).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Animation animation = new Animation();
            animation.name = anim_name;
            animation.fps = fps;

            Skeleton skeleton = null;

            bool is_time_or_end = false;
            bool is_time = false;

            foreach (string line in lines)
            {
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words[0] == "skeleton")
                {
                    is_time_or_end = true;
                    is_time = false;
                    continue;
                }

                if (is_time)
                {
                    if (words[0] == "time" || words[0] == "end")
                    {
                        is_time = false;
                        is_time_or_end = true;
                    }
                }
                
                if (is_time_or_end) {
                    if (skeleton != null)
                    {
                        animation.times.Add(skeleton);
                    }
                    
                    if (words[0] == "time")
                    {
                        skeleton = new Skeleton();

                        is_time_or_end = false;
                        is_time = true;

                        continue;
                    }

                    if (words[0] == "end")
                    {
                        is_time_or_end = false;
                        animations.Add(animation);
                        break;
                    }
                }

                if (is_time)
                {
                    skeleton.bones.Add(new Bone(new vec3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])), new vec3(float.Parse(words[4]), float.Parse(words[5]), float.Parse(words[6]))));
                }
            }

            return true;
        }

        Animation curr_animation;
        public void SetAnimation(string anim_name)
        {
            curr_animation = null;

            foreach (Animation animation in animations)
            {
                if (animation.name == anim_name)
                {
                    curr_animation = animation;
                    return;
                }
            }
        }

        public float GetFullTime()
        {
            return ((float)(curr_animation.times.Count() - 1) / curr_animation.fps);
        }

        public void SetTime(float time)
        {
            // Set Skeleton
            int start = (int)Math.Floor((double)(time * curr_animation.fps));
            int end = (int)Math.Ceiling((double)(time * curr_animation.fps));

            if (start == end)
            {
                CalcNewSkeleton(curr_animation.times[start], curr_animation.times[end], 0.0f);
            }
            else
            {
                float skeletonT1 = (float)start / curr_animation.fps;
                float skeletonT2 = (float)end / curr_animation.fps;
                float diff1 = skeletonT2 - skeletonT1;
                float diff2 = time - skeletonT1;
                float dt = diff2 / diff1;
                CalcNewSkeleton(curr_animation.times[start], curr_animation.times[end], dt);
            }

            // Update Matrices
            UpdateMatrices();
        }

        void CalcNewSkeleton(Skeleton start, Skeleton end, float dt)
        {
            for (int i = 0; i < (int)current_skeleton.bones.Count(); i++)
            {
                // translate
                current_skeleton.bones[i].translate = (start.bones[i].translate * (1.0f - dt)) + (end.bones[i].translate * dt);
                // rotate
                current_skeleton.bones[i].rotate = (start.bones[i].rotate * (1.0f - dt)) + (end.bones[i].rotate * dt);
            }
        }

        void UpdateMatrices()
        {
            // mátrixok kiszámítása rekurzívan
            for (int i = 0; i < current_skeleton.bones.Count(); i++)
            {
                transform[i] = GetMatrix(i);
            }
        }

        public mat4 GetReferenceMatrix(int id)
        {
            // ha a gyökérnél vagyunk, akkor egységmátrix
            if (id == -1) return mat4.identity();
            
            // változások a szülõhöz képest
            vec3 tr = reference_skeleton.bones[id].translate; // eltolás
            vec3 rot = reference_skeleton.bones[id].rotate; // forgatás
            // lokális és a szülõ mátrixok egybe gyúrása
            mat4 local = glm.translate(mat4.identity(), tr) * glm.rotate(rot.z, new vec3(0, 0, 1)) * glm.rotate(rot.y, new vec3(0, 1, 0)) * glm.rotate(rot.x, new vec3(1, 0, 0));
            mat4 parent_global = GetReferenceMatrix(nodes[id].parent_id);

            return (parent_global * local);
        }

        public mat4 GetMatrix(int id)
        {
            // ha a gyökérnél vagyunk, akkor egységmátrix
            if (id == -1) return mat4.identity();

            // változások a szülõhöz képest
            vec3 tr = current_skeleton.bones[id].translate; // eltolás
            vec3 rot = current_skeleton.bones[id].rotate; // forgatás
            // lokális és a szülõ mátrixok egybe gyúrása
            mat4 local = glm.translate(mat4.identity(), tr) * glm.rotate(rot.z, new vec3(0, 0, 1)) * glm.rotate(rot.y, new vec3(0, 1, 0)) * glm.rotate(rot.x, new vec3(1, 0, 0));
            mat4 parent_global = GetMatrix(nodes[id].parent_id);
            
            return (parent_global * local);
        }

        public void Release()
        {
            // nodes
            nodes.Clear();

            ;
        }


    }
}
