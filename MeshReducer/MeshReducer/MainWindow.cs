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

using MeshReducer.OBJLoader;

namespace MeshReducer
{
    public partial class MainWindow : Form
    {
        private static IntPtr hDC;
        private static IntPtr hRC;

        private OBJLoader.OBJLoader obj;
        private bool is_loaded = false;

        public MainWindow()
        {
            InitializeComponent();

            obj = null;
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
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


            Gl.glViewport(0, tabControl1.Height, ClientSize.Width, ClientSize.Height - menuStrip1.Height - tabControl1.Height);
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
            Glu.gluPerspective(45, ClientSize.Width / (double)(ClientSize.Height - menuStrip1.Height), 1, 100000);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluLookAt(0,0,10000, 0,0,0, 0,1,0);

            if (is_loaded) {
                Gl.glPushMatrix();

                Gl.glRotatef(trackBar_rotate_z.Value, 0, 0, 1);
                Gl.glRotatef(trackBar_rotate_y.Value, 0, 1, 0);
                Gl.glRotatef(trackBar_rotate_x.Value, 1, 0, 0);

                foreach (OBJLoader.OBJLoader.Material material in obj.materials) {

                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.id_texture[0]);
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    for (int i = 0; i < material.indices.Count; i++)
                    {
                        vec3 v = obj.vertices[ material.indices[i].id_vertex];
                        vec2 t = obj.text_coords[material.indices[i].id_textcoord];
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
                    Gl.glDeleteTextures(1, material.id_texture);
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

            System.Console.WriteLine("Extension: " + extension);
            System.Console.WriteLine("fullpath: " + fullpath);
            System.Console.WriteLine("filename: " + filename);
            System.Console.WriteLine("directory: " + directory);

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

                Bitmap textureImage = null;
                try
                {
                    textureImage = new Bitmap(image_filename);
                }
                catch
                {
                    return;
                }

                textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);
                BitmapData bitmapData = textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                Gl.glGenTextures(1, material.id_texture);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.id_texture[0]);

                float[] maxAnisotropy = new float[1];
                Gl.glGetFloatv(Gl.GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy);
                Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy[0]);

                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
                Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA8, textureImage.Width, textureImage.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

                textureImage.UnlockBits(bitmapData);
                textureImage.Dispose();
            }

            progressBar_load_obj.Value = progressBar_load_obj.Maximum;
            progressBar_load_obj.Update();
            label_load_obj.Text = "100 %";
            label_load_obj.Update();

            trackBar_rotate_x.Value = 0;
            trackBar_rotate_y.Value = 0;
            trackBar_rotate_z.Value = 0;

            is_loaded = true;
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
    }
}
