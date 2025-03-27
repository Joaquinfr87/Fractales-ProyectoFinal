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

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawMandelbrot();
        }

        /*private void DrawMandelbrot()
        {
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            bmp = new Bitmap(width, height);
            for (int xc = 0; xc < width; xc++)
            {
                for (int yc = 0; yc < height; yc++)
                {
                    double nReal = (xc - width / 1.2d) * 4d / (width * 5d);
                    double nImaginario = (yc - height / -2.543d) * 4d / (height * 5d);
                    double x = 0, y = 0;
                    int i = 0;
                    while (i < 5000 && (x * x + y * y) < 4) 
                    {
                        double xTemp = (x * x) - (y * y) + nReal;
                        y = 2d * x * y + nImaginario;
                        x = xTemp;
                        i++;
                    }
                    if (i >= 0 && i < 750)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(4 * i % 255, i % 3 * 30, 50));
                    }
                    else if (i >= 750 && i < 1500)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(100, i % 3 * 50, 255));
                    }
                    else if (i >= 1500 && i < 2250)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(i * 4 % 255, i % 247, i % 237));
                    }
                    else if (i >= 2250 && i < 3000)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(i % 255, 77, 0));
                    }
                    else if (i >= 3000 && i <3750)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(255, i % 2 * 255, 3 * i % 255));
                    }
                    else if (i >= 3750 && i < 4500)
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(4 * i % 255, i % 3 * 30, 50));
                    }
                    else
                    {
                        bmp.SetPixel(xc, yc, Color.FromArgb(i % 18, i % 37, i % 177));
                    }
                }
            }
            pictureBox1.Image = bmp;
        }*/
        private void DrawMandelbrot()
        {
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            bmp = new Bitmap(width, height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int byteCount = bmpData.Stride * bmp.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bmpData.Scan0;

            Parallel.For(0, height, yc =>
            {
                int yPos = yc * bmpData.Stride;
                for (int xc = 0; xc < width; xc++)
                {
                    double nReal = (xc - width / 1.2d) * 4d / (width * 5d);
                    double nImaginario = (yc - height / -2.543d) * 4d / (height * 5d);
                    double x = 0, y = 0;
                    int i = 0;
                    while (i < 5000 && (x * x + y * y) < 4)
                    {
                        double xTemp = (x * x) - (y * y) + nReal;
                        y = 2d * x * y + nImaginario;
                        x = xTemp;
                        i++;
                    }
                    Color color = GetColor(i);
                    int pixelIndex = yPos + xc * bytesPerPixel;
                    pixels[pixelIndex] = color.B;
                    pixels[pixelIndex + 1] = color.G;
                    pixels[pixelIndex + 2] = color.R;
                    pixels[pixelIndex + 3] = color.A;
                }
            });

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            bmp.UnlockBits(bmpData);
            pictureBox1.Image = bmp;
        }
        private Color GetColor(int i)
        {
            if (i >= 0 && i < 750)
            {
                return Color.FromArgb(4 * i % 255, i % 3 * 30, 50);
            }
            else if (i >= 750 && i < 1500)
            {
                return Color.FromArgb(100, i % 3 * 50, 255);
            }
            else if (i >= 1500 && i < 2250)
            {
                return Color.FromArgb(i * 4 % 255, i % 247, i % 237);
            }
            else if (i >= 2250 && i < 3000)
            {
                return Color.FromArgb(i % 255, 77, 0);
            }
            else if (i >= 3000 && i < 3750)
            {
                return Color.FromArgb(255, i % 2 * 255, 3 * i % 255);
            }
            else if (i >= 3750 && i < 4500)
            {
                return Color.FromArgb(4 * i % 255, i % 3 * 30, 50);
            }
            else
            {
                return Color.FromArgb(i % 18, i % 37, i % 177);
            }
        }
    }
}
