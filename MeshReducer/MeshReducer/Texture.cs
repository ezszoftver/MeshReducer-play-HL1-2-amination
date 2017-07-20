using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using Tao.OpenGl;

namespace MeshReducer.Texture
{
    class Texture
    {
        public int[] id = null;

        public Texture()
        {
            id = new int[1];
        }

        public bool Load(string filename)
        {
            Bitmap textureImage = null;
            try
            {
                textureImage = new Bitmap(filename);
            }
            catch
            {
                return false;
            }

            textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);
            BitmapData bitmapData = textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Gl.glGenTextures(1, id);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id[0]);

            float[] maxAnisotropy = new float[1];
            Gl.glGetFloatv(Gl.GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, maxAnisotropy[0]);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA8, textureImage.Width, textureImage.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

            textureImage.UnlockBits(bitmapData);
            textureImage.Dispose();

            return true;
        }

        public void Release()
        {
            if (id != null)
            {
                Gl.glDeleteTextures(1, id);
            }
        }
    }
}
