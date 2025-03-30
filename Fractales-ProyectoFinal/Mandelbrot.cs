using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractales_ProyectoFinal
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(758, 729);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 753);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
    }
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private const int maxIterations = 5000;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawMandelbrot();
        }
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

                    int iterations = MandelbrotRecursivo(nReal, nImaginario, 0, 0, 0);
                    Color color = GetColor(iterations);
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
        private int MandelbrotRecursivo(double x, double y, double zx, double zy, int iter)
        {
            if (iter >= maxIterations || (zx * zx + zy * zy) >= 4)
                return iter;

            double newX = (zx * zx) - (zy * zy) + x;
            double newY = 2 * zx * zy + y;

            return MandelbrotRecursivo(x, y, newX, newY, iter + 1);
        }

        private Color GetColor(int i)
        {
            if (i < 750) return Color.FromArgb(4 * i % 255, i % 3 * 30, 50);
            if (i < 1500) return Color.FromArgb(100, i % 3 * 50, 255);
            if (i < 2250) return Color.FromArgb(i * 4 % 255, i % 247, i % 237);
            if (i < 3000) return Color.FromArgb(i % 255, 77, 0);
            if (i < 3750) return Color.FromArgb(255, i % 2 * 255, 3 * i % 255);
            return Color.FromArgb(i % 18, i % 37, i % 177);
        }
    }
}
