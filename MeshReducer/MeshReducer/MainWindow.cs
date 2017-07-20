using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using Tao.Platform.Windows;
using GlmNet;

using MeshReducer.Camera;
using MeshReducer.OBJLoader;
using MeshReducer.SMDLoader;
using MeshReducer.Texture;

namespace MeshReducer
{
    public partial class MainWindow : Form, IMessageFilter
    {
        private static IntPtr hDC;
        private static IntPtr hRC;
        DateTime elapsed_datetime;
        DateTime current_datetime;
        Camera.Camera camera;

        private OBJLoader.OBJLoader obj;
        private SMDLoader.SMDLoader smd;
        private bool is_loaded = false;

        vec3 center;
        float radius;

        public MainWindow()
        {
            InitializeComponent();

            Application.AddMessageFilter(this);

            obj = null;
            smd = null;
            center = new vec3(0,0,0);
            radius = 1.0f;
            camera = new Camera.Camera();
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void ResizeComponents()
        {
            // tab
            tabControl1.SetBounds(0, ClientSize.Height - tabControl1.Height, ClientSize.Width, tabControl1.Height);

            // loadobjbox progressbar
            {
                // load
                progressBar_load_obj.Width = tabControl1.Width - 120;
                label_load_obj.Location = new Point(progressBar_load_obj.Location.X + ((progressBar_load_obj.Width - label_load_obj.Width) / 2), progressBar_load_obj.Location.Y + ((progressBar_load_obj.Height - label_load_obj.Height) / 2));
                // save
                progressBar_save_obj.Width = tabControl1.Width - 120;
                label_save_obj.Location = new Point(progressBar_save_obj.Location.X + ((progressBar_save_obj.Width - label_save_obj.Width) / 2), progressBar_save_obj.Location.Y + ((progressBar_save_obj.Height - label_save_obj.Height) / 2));
            }

            // groupbox options
            {
                groupBox_options_reduce.Width = ClientSize.Width - groupBox3.Width - 20;

                // reduce trackbar
                {
                    trackBar_reduce_percent.Width = tabControl1.Width - 280;
                    Point p = button_calc.Location;
                    button_calc.Location = new Point(tabControl1.Width - 263, p.Y);
                }

                // reduce progressbar
                {
                    progressBar_reduce.Width = tabControl1.Width - 190;
                    label_reduce.Location = new Point(progressBar_reduce.Location.X + ((progressBar_reduce.Width - label_reduce.Width) / 2), progressBar_reduce.Location.Y + ((progressBar_reduce.Height - label_reduce.Height) / 2));
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ResizeComponents();

            // update
            elapsed_datetime = current_datetime;
            current_datetime = DateTime.Now;
            TimeSpan diff = current_datetime - elapsed_datetime;
            float dt = (float)diff.TotalMilliseconds / 1000.0f;

            // camera
            // keyboard
            if (key_pressed_up) camera.MoveUp(dt);
            if (key_pressed_down) camera.MoveDown(dt);
            if (key_pressed_right) camera.MoveRight(dt);
            if (key_pressed_left) camera.MoveLeft(dt);
            // mouse
            if (mouse_pressed_right_button)
            {
                int center_x = this.Bounds.X + (this.Bounds.Width / 2);
                int center_y = this.Bounds.Y + (this.Bounds.Height / 2);

                int diff_x = Cursor.Position.X - center_x;
                int diff_y = Cursor.Position.Y - center_y;
                Cursor.Position = new Point(center_x, center_y);

                camera.RotateDir((float)diff_x, (float)diff_y);
            }
            camera.Update();

            vec2 pos = new vec2(0, tabControl1.Height);
            vec2 size = new vec2(ClientSize.Width, ClientSize.Height - menuStrip1.Height - tabControl1.Height);
            Gl.glViewport((int)pos.x, (int)pos.y, (int)size.x, (int)size.y);

            Gl.glClearColor(0.5f, 0.5f, 1.0f, 0.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

//            Gl.glBegin(Gl.GL_TRIANGLES);
//            {
//                Gl.glColor3f(1, 0, 0); Gl.glVertex2f(+0, +1);
//                Gl.glColor3f(0, 1, 0); Gl.glVertex2f(-1, -1);
//                Gl.glColor3f(0, 0, 1); Gl.glVertex2f(+1, -1);
//            }
//            Gl.glEnd();

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, size.x / size.y, radius / 1000.0f, radius * 10.0f);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            vec3 at = camera.pos + camera.dir;
            Glu.gluLookAt(camera.pos.x, camera.pos.y, camera.pos.z, at.x, at.y, at.z, camera.up.x, camera.up.y, camera.up.z);

            if (is_loaded) {
                Gl.glPushMatrix();

                Gl.glRotatef(trackBar_rotate_z.Value, 0, 0, 1);
                Gl.glRotatef(trackBar_rotate_y.Value, 0, 1, 0);
                Gl.glRotatef(trackBar_rotate_x.Value, 1, 0, 0);

                // OBJ
                /*foreach (OBJLoader.OBJLoader.Material material in obj.materials) {

                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    for (int i = 0; i < material.indices.Count; i++)
                    {
                        vec3 v = obj.vertices[ material.indices[i].id_vertex];
                        vec2 t = obj.text_coords[material.indices[i].id_textcoord];
                        Gl.glTexCoord2f(t.x, t.y);
                        Gl.glVertex3f(v.x, v.y, v.z);
                    }
                    Gl.glEnd();

                }*/

                // SMD
                foreach (SMDLoader.SMDLoader.Material material in smd.materials)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.texture.id[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (SMDLoader.SMDLoader.Vertex vertex in material.vertices)
                    {
                        vec3 v = vertex.vertex;
                        vec2 t = vertex.textcoords;
                        Gl.glTexCoord2f(t.x, t.y);
                        Gl.glVertex3f(v.x, v.y, v.z);
                    }
                    Gl.glEnd();
                }

                Gl.glPopMatrix();
            }

            Gdi.SwapBuffers(hDC);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
            pfd.nSize = (short)Marshal.SizeOf(pfd);                            // Size Of This Pixel Format Descriptor
            pfd.nVersion = 1;                                                   // Version Number
            pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
                Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGL
                Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
            pfd.iPixelType = (byte)Gdi.PFD_TYPE_RGBA;                          // Request An RGBA Format
            pfd.cColorBits = (byte)32;                                       // Select Our Color Depth
            pfd.cRedBits = 0;                                                   // Color Bits Ignored
            pfd.cRedShift = 0;
            pfd.cGreenBits = 0;
            pfd.cGreenShift = 0;
            pfd.cBlueBits = 0;
            pfd.cBlueShift = 0;
            pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
            pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
            pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
            pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
            pfd.cAccumGreenBits = 0;
            pfd.cAccumBlueBits = 0;
            pfd.cAccumAlphaBits = 0;
            pfd.cDepthBits = 24;                                                // 16Bit Z-Buffer (Depth Buffer)
            pfd.cStencilBits = 0;                                               // No Stencil Buffer
            pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
            pfd.iLayerType = (byte)Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
            pfd.bReserved = 0;                                                  // Reserved
            pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
            pfd.dwVisibleMask = 0;
            pfd.dwDamageMask = 0;

            hDC = User.GetDC(this.Handle);                                      // Attempt To Get A Device Context
            if (hDC == IntPtr.Zero)
            {                                            // Did We Get A Device Context?                                                // Reset The Display
                MessageBox.Show("Can't Create A GL Device Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            int pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);                  // Attempt To Find An Appropriate Pixel Format
            if (pixelFormat == 0)
            {                                              // Did Windows Find A Matching Pixel Format?                                                 // Reset The Display
                MessageBox.Show("Can't Find A Suitable PixelFormat.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if (!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd))
            {                // Are We Able To Set The Pixel Format?                                              // Reset The Display
                MessageBox.Show("Can't Set The PixelFormat.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            hRC = Wgl.wglCreateContext(hDC);                                    // Attempt To Get The Rendering Context
            if (hRC == IntPtr.Zero)
            {                                            // Are We Able To Get A Rendering Context?                                                // Reset The Display
                MessageBox.Show("Can't Create A GL Rendering Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if (!Wgl.wglMakeCurrent(hDC, hRC))
            {                                 // Try To Activate The Rendering Context                                                // Reset The Display
                MessageBox.Show("Can't Activate The GL Rendering Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            this.timer1.Start();
            current_datetime = elapsed_datetime = DateTime.Now;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            is_loaded = false;
            progressBar_load_obj.Value = 0;
            label_load_obj.Text = "0 %";

            if (obj != null)
            {
                foreach (OBJLoader.OBJLoader.Material material in obj.materials)
                {
                    material.texture.Release();
                }

                obj.Release();
                obj = null;
                System.GC.Collect();
            }
            
            obj = new OBJLoader.OBJLoader();
            if (obj == null)
            {
                return;
            }

            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open OBJ File";
            theDialog.Filter = "OBJ files|*.obj";
            theDialog.InitialDirectory = @"./";
            if (theDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            
            string fullpath = theDialog.FileName;
            string filename = theDialog.SafeFileName;
            string directory = Path.GetDirectoryName(fullpath);
            string extension = Path.GetExtension(fullpath).ToLower();

            if (extension != ".obj") {
                return;
            }

            if (!obj.Load(directory, filename)) {
                return;
            }

            progressBar_load_obj.Maximum = 10 + obj.materials.Count;
            progressBar_load_obj.Value = 10;

            float step = 100.0f / (10.0f + (float)obj.materials.Count);
            float percent = 10.0f * step;
            label_load_obj.Text = percent + " %";

            // load textures
            foreach (OBJLoader.OBJLoader.Material material in obj.materials)
            {
                progressBar_load_obj.Value++;
                progressBar_load_obj.Update();

                percent += step;
                label_load_obj.Text = (int)percent + " %";
                label_load_obj.Update();

                string image_filename = "";
                if (material.texture_filename.Contains(":"))
                {
                    image_filename = material.texture_filename;
                }
                else {
                    image_filename = directory + @"\" + material.texture_filename;
                }

                if (!material.texture.Load(image_filename))
                {
                    return;
                }
            }

            progressBar_load_obj.Value = progressBar_load_obj.Maximum;
            progressBar_load_obj.Update();
            label_load_obj.Text = "100 %";
            label_load_obj.Update();

            trackBar_rotate_x.Value = 0;
            trackBar_rotate_y.Value = 0;
            trackBar_rotate_z.Value = 0;

            // set up camera
            center = new vec3((obj.min + obj.max) / 2.0f);
            radius = glm_length(center, obj.max);
            camera.pos = center + (new vec3(0, 0, 1) * radius * 2.0f);
            camera.dir = glm.normalize(center - camera.pos);
            camera.SetVelocity(radius / 5.0f);

            is_loaded = true;
        }

        float glm_length(vec3 a, vec3 b)
        {
            vec3 v = b - a;
            float length = (float)Math.Sqrt((float)(v.x*v.x + v.y*v.y + v.z*v.z));
            return length;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void progressBar_reduce_Click(object sender, EventArgs e)
        {

        }

        bool mouse_pressed_right_button = false;
        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int center_x = this.Bounds.X + (this.Bounds.Width / 2);
                int center_y = this.Bounds.Y + (this.Bounds.Height / 2);

                Cursor.Position = new Point(center_x, center_y);
                mouse_pressed_right_button = true;
            }
        }

        private void MainWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mouse_pressed_right_button = false;
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {           
        }

        bool key_pressed_up = false;
        bool key_pressed_down = false;
        bool key_pressed_right = false;
        bool key_pressed_left = false;

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        public bool PreFilterMessage(ref Message m)
        {
            // up
            if (m.Msg == WM_KEYDOWN && (Keys)m.WParam.ToInt32() == Keys.W)
            {
                key_pressed_up = true;
                return true;
            }
            if (m.Msg == WM_KEYUP && (Keys)m.WParam.ToInt32() == Keys.W)
            {
                key_pressed_up = false;
                return true;
            }

            // down
            if (m.Msg == WM_KEYDOWN && (Keys)m.WParam.ToInt32() == Keys.S)
            {
                key_pressed_down = true;
                return true;
            }
            if (m.Msg == WM_KEYUP && (Keys)m.WParam.ToInt32() == Keys.S)
            {
                key_pressed_down = false;
                return true;
            }

            // right
            if (m.Msg == WM_KEYDOWN && (Keys)m.WParam.ToInt32() == Keys.D)
            {
                key_pressed_right = true;
                return true;
            }
            if (m.Msg == WM_KEYUP && (Keys)m.WParam.ToInt32() == Keys.D)
            {
                key_pressed_right = false;
                return true;
            }

            // left
            if (m.Msg == WM_KEYDOWN && (Keys)m.WParam.ToInt32() == Keys.A)
            {
                key_pressed_left = true;
                return true;
            }
            if (m.Msg == WM_KEYUP && (Keys)m.WParam.ToInt32() == Keys.A)
            {
                key_pressed_left = false;
                return true;
            }

            return false;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            key_pressed_up = false;
            key_pressed_down = false;
            key_pressed_right = false;
            key_pressed_left = false;

            mouse_pressed_right_button = false;
        }

        private void MainWindow_Deactivate(object sender, EventArgs e)
        {
            key_pressed_up = false;
            key_pressed_down = false;
            key_pressed_right = false;
            key_pressed_left = false;

            mouse_pressed_right_button = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            is_loaded = false;

            if (smd != null)
            {
                foreach (SMDLoader.SMDLoader.Material material in smd.materials)
                {
                    material.texture.Release();
                }

                smd.Release();
                smd = null;
                System.GC.Collect();
            }

            smd = new SMDLoader.SMDLoader();
            if (smd == null)
            {
                return;
            }

            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open SMD Reference File";
            theDialog.Filter = "SMD files|*.smd";
            theDialog.InitialDirectory = @"./";
            if (theDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fullpath = theDialog.FileName;
            string filename = theDialog.SafeFileName;
            string directory = Path.GetDirectoryName(fullpath);
            string extension = Path.GetExtension(fullpath).ToLower();

            if (extension != ".smd")
            {
                return;
            }

            if (!smd.LoadReference(directory, filename))
            {
                return;
            }

            foreach (SMDLoader.SMDLoader.Material material in smd.materials)
            {
                string image_filename = directory + @"\" + material.texture_name;
                if (!material.texture.Load(image_filename))
                {
                    return;
                }
            }

            trackBar_rotate_x.Value = 0;
            trackBar_rotate_y.Value = 0;
            trackBar_rotate_z.Value = 0;

            // set up camera
            center = new vec3((smd.min + smd.max) / 2.0f);
            radius = glm_length(center, smd.max);
            camera.pos = center + (new vec3(0, 0, 1) * radius * 2.0f);
            camera.dir = glm.normalize(center - camera.pos);
            camera.SetVelocity(radius / 5.0f);

            is_loaded = true;
        }
    }
}
