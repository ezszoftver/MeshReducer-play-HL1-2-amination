using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Numerics;

using Tao.OpenGl;

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
		    public Bone(Vector3 translate, Vector3 rotate)
            {
                this.translate = translate;
                this.rotate = rotate;
            }

            public Vector3 translate;
            public Vector3 rotate;
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
        public Matrix4x4[] transform_inverse;
        Skeleton current_skeleton;
        public Matrix4x4[] transform;

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
            }

            public void AddMatrix(int matrix_id, float weight)
            {
                matrices.Add(new MatrixIdAndWeight(matrix_id, weight));
            }

            public Vector3 vertex;
            public Vector2 textcoords;
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
        public Vector3 min, max;

        public SMDLoader()
        {
            nodes = new List<Node>();
            animations = new List<Animation>();
            materials = new List<Material>();
            material_to_id = new Dictionary<string, int>();
            reference_skeleton = new Skeleton();
            current_skeleton = new Skeleton();
            min = new Vector3(0, 0, 0);
            max = new Vector3(0, 0, 0);
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

                    reference_skeleton.bones.Add(new Bone(new Vector3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])), new Vector3(float.Parse(words[4]), float.Parse(words[5]), float.Parse(words[6]))));
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
                        Vector3 v = new Vector3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                        // textcoords
                        Vector2 t = new Vector2(float.Parse(words[7]), float.Parse(words[8]));

                        Vertex vertex = new Vertex(v, t);

                        // one matrix
                        vertex.AddMatrix(matrix_id, weight);

                        // add
                        materials[mat_id].vertices.Add(vertex);
                    }
                    else // HL2
                    {
                        // vertex
                        Vector3 v = new Vector3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                        // textcoords
                        Vector2 t = new Vector2(float.Parse(words[7]), float.Parse(words[8]));

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
            min = new Vector3(+1000000.0f, +1000000.0f, +1000000.0f);
            max = new Vector3(-1000000.0f, -1000000.0f, -1000000.0f);
            foreach (Material material in materials)
            {
                foreach (Vertex vertex in material.vertices)
                {
                    Vector3 v = vertex.vertex;

                    // min
                    if (v.X < min.X) { min.X = v.X; }
                    if (v.Y < min.Y) { min.Y = v.Y; }
                    if (v.Z < min.Z) { min.Z = v.Z; }
                    // max
                    if (max.X < v.X) { max.X = v.X; }
                    if (max.Y < v.Y) { max.Y = v.Y; }
                    if (max.Z < v.Z) { max.Z = v.Z; }
                }
            }

            // init current skeleton
            for (int i = 0; i < reference_skeleton.bones.Count(); i++)
            {
                current_skeleton.bones.Add(new Bone(new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
            }

            // calc matrices
            transform = new Matrix4x4[reference_skeleton.bones.Count()];
            transform_inverse = new Matrix4x4[reference_skeleton.bones.Count()];
            for (int i = 0; i < reference_skeleton.bones.Count(); i++)
            {
                Matrix4x4.Invert(GetReferenceMatrix(i), out transform_inverse[i]);
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
                    skeleton.bones.Add(new Bone(new Vector3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])), new Vector3(float.Parse(words[4]), float.Parse(words[5]), float.Parse(words[6]))));
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
                double skeletonT1 = (double)start / curr_animation.fps;
                double skeletonT2 = (double)end / curr_animation.fps;
                double diff1 = skeletonT2 - skeletonT1;
                double diff2 = time - skeletonT1;
                double dt = diff2 / diff1;
                CalcNewSkeleton(curr_animation.times[start], curr_animation.times[end], (float)dt);
            }

            // Update Matrices
            UpdateMatrices();
        }

        private float GetSignedRad(float alfa, float beta)
        {
            float difference = beta - alfa;
            while (difference < -(float)Math.PI) difference += 2.0f * (float)Math.PI;
            while (difference > (float)Math.PI) difference -= 2.0f * (float)Math.PI;
            return difference;
        }

        void CalcNewSkeleton(Skeleton start, Skeleton end, float dt)
        {
            for (int i = 0; i < (int)current_skeleton.bones.Count(); i++)
            {
                // translate
                current_skeleton.bones[i].translate = (start.bones[i].translate * (1.0f - dt)) + (end.bones[i].translate * dt);
                // rotate
                // X
                float rotate_x = GetSignedRad(start.bones[i].rotate.X, end.bones[i].rotate.X);
                current_skeleton.bones[i].rotate.X = start.bones[i].rotate.X + (rotate_x * dt);
                // Y
                float rotate_y = GetSignedRad(start.bones[i].rotate.Y, end.bones[i].rotate.Y);
                current_skeleton.bones[i].rotate.Y = start.bones[i].rotate.Y + (rotate_y * dt);
                // Z
                float rotate_z = GetSignedRad(start.bones[i].rotate.Z, end.bones[i].rotate.Z);
                current_skeleton.bones[i].rotate.Z = start.bones[i].rotate.Z + (rotate_z * dt);
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

        public Matrix4x4 GetReferenceMatrix(int id)
        {
            // ha a gyökérnél vagyunk, akkor egységmátrix
            if (id == -1) return Matrix4x4.Identity;

            // változások a szülõhöz képest
            Vector3 tr = reference_skeleton.bones[id].translate; // eltolás
            Vector3 rot = reference_skeleton.bones[id].rotate; // forgatás
            // lokális és a szülõ mátrixok egybe gyúrása
            Matrix4x4 local = Matrix4x4.Multiply(Matrix4x4.Multiply(Matrix4x4.Multiply(Matrix4x4.CreateRotationX(rot.X), Matrix4x4.CreateRotationY(rot.Y)), Matrix4x4.CreateRotationZ(rot.Z)), Matrix4x4.CreateTranslation(tr));
            Matrix4x4 parent_global = GetReferenceMatrix(nodes[id].parent_id);

            return Matrix4x4.Multiply(local, parent_global);
        }

        public Matrix4x4 GetMatrix(int id)
        {
            // ha a gyökérnél vagyunk, akkor egységmátrix
            if (id == -1) return Matrix4x4.Identity;

            // változások a szülõhöz képest
            Vector3 tr = current_skeleton.bones[id].translate; // eltolás
            Vector3 rot = current_skeleton.bones[id].rotate; // forgatás
            // lokális és a szülõ mátrixok egybe gyúrása
            Matrix4x4 local = Matrix4x4.Multiply(Matrix4x4.Multiply(Matrix4x4.Multiply(Matrix4x4.CreateRotationX(rot.X), Matrix4x4.CreateRotationY(rot.Y)), Matrix4x4.CreateRotationZ(rot.Z)), Matrix4x4.CreateTranslation(tr));
            Matrix4x4 parent_global = GetMatrix(nodes[id].parent_id);

            return Matrix4x4.Multiply(local, parent_global);
        }

        public void Release()
        {
            // nodes
            nodes.Clear();

            ;
        }


    }
}
