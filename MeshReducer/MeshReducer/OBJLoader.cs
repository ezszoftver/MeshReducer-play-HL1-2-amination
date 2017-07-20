using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tao.OpenGl;
using GlmNet;

using MeshReducer.Texture;

namespace MeshReducer.OBJLoader
{
    class OBJLoader
    {
        public vec3 min, max;
        public List<vec3> vertices;
        public List<vec2> text_coords;

        private Dictionary<string, UInt32> material_to_id;
        private Dictionary<string, string> material_to_texture;

        public class Vertex
        {
            public Int32 id_vertex;
            public Int32 id_textcoord;

            public Vertex(Int32 id_vertex, Int32 id_textcoord)
            {
                this.id_vertex = id_vertex;
                this.id_textcoord = id_textcoord;
            }
        }

        public class Material
        {
            public string texture_filename;
            public Texture.Texture texture;

            public List<Vertex> indices;

            public Material(string texture_filename)
            {
                indices = new List<Vertex>();
                this.texture_filename = texture_filename;
                texture = new Texture.Texture();
            }
            
            public void Release()
            {
                indices.Clear();
                indices = null;
            }
        };

        public List<Material> materials;

        public OBJLoader()
        {
            min = new vec3(0, 0, 0);
            max = new vec3(0, 0, 0);
            vertices = new List<vec3>();
            text_coords = new List<vec2>();
            material_to_id = new Dictionary<string, UInt32>();
            material_to_texture = new Dictionary<string, string>();
            materials = new List<Material>();
        }

        public Boolean Load(string directory, string filename)
        {
            string[] lines = File.ReadAllText(directory + @"\" + filename).Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            min = new vec3(+1000000.0f, +1000000.0f, +1000000.0f);
            max = new vec3(-1000000.0f, -1000000.0f, -1000000.0f);
            UInt32 material_id = 0;

            foreach (string line in lines) {
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (words[0]) {
                    case ("#"): {
                            break;
                        }
                    case ("v"): {
                            vec3 v = new vec3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                            vertices.Add(v);

                            // min
                            if (v.x < min.x) { min.x = v.x; }
                            if (v.y < min.y) { min.y = v.y; }
                            if (v.z < min.z) { min.z = v.z; }
                            // max
                            if (max.x < v.x) { max.x = v.x; }
                            if (max.y < v.y) { max.y = v.y; }
                            if (max.z < v.z) { max.z = v.z; }

                            break;
                        }
                    case ("vt"): {
                            vec2 vt = new vec2(float.Parse(words[1]), float.Parse(words[2]));
                            text_coords.Add(vt);
                            break;
                        }
                    case ("mtllib"): {
                            string[] mtl_lines = File.ReadAllText(directory + @"\" + words[1]).Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                            string material = "";
                            string texture_name = "";
                            bool is_save = true;

                            foreach (string mtl_line in mtl_lines) {
                                string[] mtl_words = mtl_line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            
                                switch (mtl_words[0]) {
                                    case ("#"): {
                                            break;
                                        }
                                    case ("newmtl"): {
                                            if (is_save == false) {
                                                texture_name = "TextureNotFound.bmp";

                                                materials.Add(new Material(texture_name));
                                                material_to_texture.Add(material, texture_name);
                                                int id = material_to_id.Count;
                                                material_to_id.Add(material, (UInt32)id);

                                                material = "";
                                            }

                                            material = mtl_words[1];
                                            is_save = false;
                                            break;
                                        }
                                    case ("map_Kd"): {
                                            texture_name = mtl_words[1];

                                            materials.Add(new Material(texture_name));
                                            material_to_texture.Add(material, texture_name);
                                            int id = material_to_id.Count;
                                            material_to_id.Add(material, (UInt32)id);
                                            
                                            material = "";
                                            is_save = true;
                                            break;
                                        }
                                }
                            }

                            break;
                        }
                    case ("usemtl"): {
                            material_id = material_to_id[words[1]];
                            break;
                        }
                    case ("f"): {
                            Vertex first = null;
                            for (int i = 1; i < words.Count(); i++) {
                                string vertex = words[i];
                                string[] vnt = vertex.Split('/');

                                Int32 v_id = Int32.Parse(vnt[0]) - 1;
                                Int32 t_id = -1;
                                if ((vnt.Count() == 2 || vnt.Count() == 3) && vnt[1].Count() > 0) {
                                    t_id = Int32.Parse(vnt[1]) - 1;
                                }

                                if (i <= 3) {
                                    if (i == 1) {
                                        first = new Vertex(v_id, t_id);
                                        materials[(int)material_id].indices.Add(first);
                                    } else {
                                        materials[(int)material_id].indices.Add(new Vertex(v_id, t_id));
                                    }
                                } else {
                                    int id_last = materials[(int)material_id].indices.Count - 1;
                                    Vertex second = materials[(int)material_id].indices[id_last];
                                    // 1
                                    materials[(int)material_id].indices.Add(first);
                                    // 2
                                    materials[(int)material_id].indices.Add(second);
                                    // 3
                                    materials[(int)material_id].indices.Add(new Vertex(v_id, t_id));
                                }
                            }
                            break;
                        }
                }
            }

            return true;
        }

        public void Release()
        {
            // vertices
            vertices.Clear();
            vertices = null;

            // text coords
            text_coords.Clear();
            text_coords = null;

            // material to id
            material_to_id.Clear();
            material_to_id = null;
            // material to texture
            material_to_texture.Clear();      
            material_to_texture = null;

            // materials
            foreach (Material material in materials) {
                material.Release();
            }
            materials.Clear();
            materials = null;
        }
    }
}
