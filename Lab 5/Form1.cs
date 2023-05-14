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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        class GLTexture
        {
            public static void LoadTexture(Bitmap bmp)
            {
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
            }
        }
        bool texture = false;
        float AngleX = 0;
        float AngleY = 0;
        float AngleZ = 0;
        const float AngleDl = 5;
        PolygonMode mode = PolygonMode.Fill;
        Bitmap bmpTex;

        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Viewport(0, 0, w, h);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(1f, 1f, 1f, 1f); // цвет фона
            GL.Enable(EnableCap.DepthTest);
            bmpTex = new Bitmap("Texture.bmp"); // изображение используемое в качестве текстуры
            GLTexture.LoadTexture(bmpTex);
            // задание параметров текстуры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            AngleX = 100;
            AngleY = 0;
        }

        public static void LoadTexture(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Rgb, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {


            GL.Translate(0, 0, -7 - 100);
            // очистка буферов цвета и глубины
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // поворот изображения
            GL.LoadIdentity();
            GL.Rotate(AngleX, 1.0, 0.0, 0.0);
            GL.Rotate(AngleY, 0.0, 1.0, 0.0);
            GL.Rotate(AngleZ, 0.0, 0.0, 1.0);

            // формирование изображения


            // включаем режим наложения текстуры при нажатии на клавишу
            if (texture)
                GL.Enable(EnableCap.Texture2D); 


            GL.Color3(Color.Red);
            GL.PolygonMode(MaterialFace.FrontAndBack, mode);

            int n = 20;
            double r;

            r = 0.1;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, -0.6);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2((double)i / n, 1);
                GL.Vertex3(x, y, -0.6);
            }
            GL.End();



            r = 0.1;
            GL.Begin(PrimitiveType.QuadStrip);
            for (int i = 0; i <= n; i++)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);

                GL.TexCoord2((double)i / n, 0); GL.Vertex3(x, y, -0.5);
                GL.TexCoord2((double)i / n, -0.6); GL.Vertex3(x, y, -0.6);
            }
            GL.End();

            r = 0.14;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, -0.5);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2((double)i / n, -0.5);
                GL.Vertex3(x, y, -0.5);
            }
            GL.End();



            r = 0.14;
            GL.Begin(PrimitiveType.QuadStrip); // задаем тип примитивов
            for (int i = 0; i <= n; i++)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                // для каждой вершины задаем сначала текстурные координаты, а затем
                // пространственные координаты
                GL.TexCoord2((double)i / n, -0.5); GL.Vertex3(x, y, -0.5);
                GL.TexCoord2((double)i / n, -0.35); GL.Vertex3(x, y, -0.35);
            }
            GL.End();

            r = 0.3;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, -0.35);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2((double)i / n, -0.35);
                GL.Vertex3(x, y, -0.35);
            }
            GL.End();


            r = 0.3;
            GL.Begin(PrimitiveType.QuadStrip); // задаем тип примитивов
            for (int i = 0; i <= n; i++)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                // для каждой вершины задаем сначала текстурные координаты, а затем
                // пространственные координаты
                GL.TexCoord2((double)i / n, -0.35); GL.Vertex3(x, y, -0.35);
                GL.TexCoord2((double)i / n, -0.15); GL.Vertex3(x, y, -0.15);
            }
            GL.End();


            r = 0.3;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, -0.15);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2((double)i / n, -0.15);
                GL.Vertex3(x, y, -0.15);
            }
            GL.End();



            r = 0.2;
            GL.Begin(PrimitiveType.QuadStrip); // задаем тип примитивов
            for (int i = 0; i <= n; i++)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                // для каждой вершины задаем сначала текстурные координаты, а затем
                // пространственные координаты
                GL.TexCoord2((double)i / n, -0.25); GL.Vertex3(x, y, -0.15);
                GL.TexCoord2((double)i / n, 1); GL.Vertex3(x, y, 0.5);
            }
            GL.End();

            r = 0.45;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, 0.5);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2(x, y);
                GL.Vertex3(x, y, 0.5);
            }
            GL.End();



            //длинная палка
            r = 0.45;
            GL.Begin(PrimitiveType.QuadStrip); // задаем тип примитивов
            for (int i = 0; i <= n; i++)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                // для каждой вершины задаем сначала текстурные координаты, а затем
                // пространственные координаты
                GL.TexCoord2((double)i / n, 0.5); GL.Vertex3(x, y, 0.5);
                GL.TexCoord2((double)i / n, 0.7); GL.Vertex3(x, y, 0.7);
            }
            GL.End();


            r = 0.45;
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, 0.7);
            for (int i = 0; i <= n; ++i)
            {
                double a = 2 * Math.PI / n * i;
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                GL.TexCoord2(x, y);
                GL.Vertex3(x, y, 0.7);
            }
            GL.End();



            GL.Disable(EnableCap.Texture2D);
            // завершение формирования изображения
            GL.Flush();
            GL.Finish();

            glControl1.SwapBuffers();
        }


        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    AngleX += AngleDl;
                    break;
                case Keys.W:
                    AngleX -= AngleDl;
                    break;
                case Keys.A:
                    AngleY += AngleDl;
                    break;
                case Keys.D:
                    AngleY -= AngleDl;
                    break;
                case Keys.F:
                    AngleZ += AngleDl;
                    break;
                case Keys.G:
                    AngleZ -= AngleDl;
                    break;
                case Keys.F1:
                    mode = PolygonMode.Fill;
                    break;
                case Keys.F2:
                    mode = PolygonMode.Line;
                    break;
                case Keys.F3:
                    mode = PolygonMode.Point;
                    break;
                case Keys.F4:
                    texture =! texture;
                    break;
            }
            glControl1.Invalidate();
        }
    }
}
