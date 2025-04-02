using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractales_ProyectoFinal
{
    public class MandelbrotForm : Form
    {
        private PictureBox pictureBox = new PictureBox();
        private Button btnZoomIn = new Button();
        private Button btnZoomOut = new Button();
        private Button btnReset = new Button();
        private Button btnUp = new Button();
        private Button btnDown = new Button();
        private Button btnLeft = new Button();
        private Button btnRight = new Button();
        private Button btnSaveState, btnUndo, btnSaveFavorite, btnLoadFavorite;
        private TextBox txtMaxIterations = new TextBox();
        private Label lblMaxIterations = new Label();
        private TextBox txtFavoriteName = new TextBox();
        private Label lblFavorite = new Label();
        private Bitmap? bmp;
        private int maxIterations = 5000;
        private double zoomFactor = 1.0;
        private double offsetX = 0, offsetY = 0;

        // Lista para historial de estados
        private List<ViewState> viewHistory = new List<ViewState>();
        // Pila para undo
        private Stack<ViewState> undoStack = new Stack<ViewState>();
        // Cola para registrar comandos
        private Queue<string> commandQueue = new Queue<string>();
        // Árbol binario para vistas favoritas  
        private BinaryTree favoritesTree = new BinaryTree();                 

        public MandelbrotForm()
        {
            InitializeComponents();
            this.Load += MandelbrotForm_Load;
        }

        private void InitializeComponents()
        {
            this.ClientSize = new Size(800, 850);
            this.Text = "Mandelbrot";

            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(780, 600);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBox);
  
            btnZoomIn.Text = "Zoom In";
            btnZoomIn.Location = new Point(10, 620);
            btnZoomIn.Click += BtnZoomIn_Click;
            this.Controls.Add(btnZoomIn);

            btnZoomOut.Text = "Zoom Out";
            btnZoomOut.Location = new Point(100, 620);
            btnZoomOut.Click += BtnZoomOut_Click;
            this.Controls.Add(btnZoomOut);

            btnReset.Text = "Reset";
            btnReset.Location = new Point(190, 620);
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);
  
            lblMaxIterations.Text = "Max Iterations:";
            lblMaxIterations.Location = new Point(10, 660);
            lblMaxIterations.AutoSize = true;
            this.Controls.Add(lblMaxIterations);

            txtMaxIterations.Text = maxIterations.ToString();
            txtMaxIterations.Location = new Point(120, 655);
            txtMaxIterations.Width = 80;
            txtMaxIterations.Leave += TxtMaxIterations_Leave;
            this.Controls.Add(txtMaxIterations);

            btnUp.Text = "Up";
            btnUp.Location = new Point(300, 620);
            btnUp.Click += BtnUp_Click;
            this.Controls.Add(btnUp);

            btnLeft.Text = "Left";
            btnLeft.Location = new Point(250, 660);
            btnLeft.Click += BtnLeft_Click;
            this.Controls.Add(btnLeft);

            btnRight.Text = "Right";
            btnRight.Location = new Point(350, 660);
            btnRight.Click += BtnRight_Click;
            this.Controls.Add(btnRight);

            btnDown.Text = "Down";
            btnDown.Location = new Point(300, 700);
            btnDown.Click += BtnDown_Click;
            this.Controls.Add(btnDown);

            btnSaveState = new Button { Text = "Save State", Location = new Point(10, 720) };
            btnSaveState.Click += BtnSaveState_Click;
            this.Controls.Add(btnSaveState);

            btnUndo = new Button { Text = "Undo", Location = new Point(110, 720) };
            btnUndo.Click += BtnUndo_Click;
            this.Controls.Add(btnUndo);

            lblFavorite = new Label { Text = "Favorite Name:", Location = new Point(10, 760), AutoSize = true };
            this.Controls.Add(lblFavorite);

            txtFavoriteName = new TextBox { Location = new Point(120, 755), Width = 100 };
            this.Controls.Add(txtFavoriteName);

            btnSaveFavorite = new Button { Text = "Save Favorite", Location = new Point(230, 755) };
            btnSaveFavorite.Click += BtnSaveFavorite_Click;
            this.Controls.Add(btnSaveFavorite);

            btnLoadFavorite = new Button { Text = "Load Favorite", Location = new Point(350, 755) };
            btnLoadFavorite.Click += BtnLoadFavorite_Click;
            this.Controls.Add(btnLoadFavorite);
        }

        private void MandelbrotForm_Load(object? sender, EventArgs e)
        {
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnZoomIn_Click(object? sender, EventArgs e)
        {
            zoomFactor *= 1.2;
            commandQueue.Enqueue("Zoom In");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnZoomOut_Click(object? sender, EventArgs e)
        {
            zoomFactor /= 1.2;
            commandQueue.Enqueue("Zoom Out");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnReset_Click(object? sender, EventArgs e)
        {
            zoomFactor = 1.0;
            offsetX = 0;
            offsetY = 0;
            maxIterations = 5000;
            txtMaxIterations.Text = maxIterations.ToString();
            commandQueue.Enqueue("Reset");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void TxtMaxIterations_Leave(object? sender, EventArgs e)
        {
            if (int.TryParse(txtMaxIterations.Text, out int newMax))
            {
                maxIterations = newMax;
                commandQueue.Enqueue("Set Max Iterations");
                SaveCurrentState();
                DrawMandelbrot();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número entero válido para las iteraciones máximas.",
                                "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaxIterations.Text = maxIterations.ToString();
            }
        }
        private void BtnUp_Click(object? sender, EventArgs e)
        {
            double moveStep = 0.1 * (4.0 / zoomFactor);
            offsetY -= moveStep;
            commandQueue.Enqueue("Move Up");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnDown_Click(object? sender, EventArgs e)
        {
            double moveStep = 0.1 * (4.0 / zoomFactor);
            offsetY += moveStep;
            commandQueue.Enqueue("Move Down");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnLeft_Click(object? sender, EventArgs e)
        {
            double moveStep = 0.1 * (4.0 / zoomFactor);
            offsetX -= moveStep;
            commandQueue.Enqueue("Move Left");
            SaveCurrentState();
            DrawMandelbrot();
        }

        private void BtnRight_Click(object? sender, EventArgs e)
        {
            double moveStep = 0.1 * (4.0 / zoomFactor);
            offsetX += moveStep;
            commandQueue.Enqueue("Move Right");
            SaveCurrentState();
            DrawMandelbrot();
        }
        private void SaveCurrentState()
        {
            var state = new ViewState(zoomFactor, offsetX, offsetY, maxIterations);
            viewHistory.Add(state);
            undoStack.Push(state);
        }
        private void BtnSaveState_Click(object sender, EventArgs e)
        {
            SaveCurrentState();
            MessageBox.Show("Estado guardado en el historial.", "Estado Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1)
            { 
                undoStack.Pop();
                ViewState prevState = undoStack.Peek();
                zoomFactor = prevState.ZoomFactor;
                offsetX = prevState.OffsetX;
                offsetY = prevState.OffsetY;
                maxIterations = prevState.MaxIterations;
                txtMaxIterations.Text = maxIterations.ToString();
                commandQueue.Enqueue("Undo");
                DrawMandelbrot();
            }
            else
            {
                MessageBox.Show("No hay más estados para deshacer.", "Undo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnSaveFavorite_Click(object sender, EventArgs e)
        {
            string favName = txtFavoriteName.Text.Trim();
            if (!string.IsNullOrEmpty(favName))
            {
                var state = new ViewState(zoomFactor, offsetX, offsetY, maxIterations);
                favoritesTree.Insert(favName, state);
                MessageBox.Show("Vista guardada como favorita.", "Favorito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                commandQueue.Enqueue("Save Favorite");
            }
            else
            {
                MessageBox.Show("Ingrese un nombre para la vista favorita.", "Favorito", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BtnLoadFavorite_Click(object sender, EventArgs e)
        {
            string favName = txtFavoriteName.Text.Trim();
            if (!string.IsNullOrEmpty(favName))
            {
                var favState = favoritesTree.Search(favName);
                if (favState != null)
                {
                    zoomFactor = favState.ZoomFactor;
                    offsetX = favState.OffsetX;
                    offsetY = favState.OffsetY;
                    maxIterations = favState.MaxIterations;
                    txtMaxIterations.Text = maxIterations.ToString();
                    commandQueue.Enqueue("Load Favorite");
                    SaveCurrentState();
                    DrawMandelbrot();
                }
                else
                {
                    MessageBox.Show("No se encontró una vista favorita con ese nombre.", "Favorito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Ingrese el nombre de la vista favorita que desea cargar.", "Favorito", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DrawMandelbrot()
        {
            try
            {
                int width = pictureBox.Width;
                int height = pictureBox.Height;
                bmp = new Bitmap(width, height);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                                                  ImageLockMode.WriteOnly, bmp.PixelFormat);
                int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int byteCount = bmpData.Stride * height;
                byte[] pixels = new byte[byteCount];
                IntPtr ptrFirstPixel = bmpData.Scan0;

                Parallel.For(0, height, yc =>
                {
                    int yPos = yc * bmpData.Stride;
                    for (int xc = 0; xc < width; xc++)
                    {
                        double nReal = (xc - width / 2.0) * (4.0 / (width * zoomFactor)) + offsetX;
                        double nImag = (yc - height / 2.0) * (4.0 / (height * zoomFactor)) + offsetY;
                        int iterations = MandelbrotRecursive(nReal, nImag, 0, 0, 0);
                        Color color = GetColor(iterations);
                        int pixelIndex = yPos + xc * bytesPerPixel;
                        pixels[pixelIndex] = color.B;
                        pixels[pixelIndex + 1] = color.G;
                        pixels[pixelIndex + 2] = color.R;
                        if (bytesPerPixel == 4)
                            pixels[pixelIndex + 3] = color.A;
                    }
                });

                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
                bmp.UnlockBits(bmpData);
                pictureBox.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dibujar Mandelbrot: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int MandelbrotRecursive(double x, double y, double zx, double zy, int iter)
        {
            if (iter >= maxIterations || (zx * zx + zy * zy) >= 4)
                return iter;
            double newZx = zx * zx - zy * zy + x;
            double newZy = 2 * zx * zy + y;
            return MandelbrotRecursive(x, y, newZx, newZy, iter + 1);
        }

        private Color GetColor(int iter)
        {
            if (iter >= maxIterations)
                return Color.Black;
            int r = (iter * 9) % 256;
            int g = (iter * 7) % 256;
            int b = (iter * 5) % 256;
            return Color.FromArgb(r, g, b);
        }
    }
    public class ViewState
    {
        public double ZoomFactor { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public int MaxIterations { get; set; }

        public ViewState(double zoom, double offsetX, double offsetY, int maxIter)
        {
            ZoomFactor = zoom;
            OffsetX = offsetX;
            OffsetY = offsetY;
            MaxIterations = maxIter;
        }

        public override string ToString()
        {
            return $"Zoom: {ZoomFactor:F2}, OffsetX: {OffsetX:F2}, OffsetY: {OffsetY:F2}, Iter: {MaxIterations}";
        }
    }
    public class BinaryTreeNode
    {
        public string Key { get; set; }
        public ViewState Value { get; set; }
        public BinaryTreeNode Left { get; set; }
        public BinaryTreeNode Right { get; set; }

        public BinaryTreeNode(string key, ViewState value)
        {
            Key = key;
            Value = value;
        }
    }
    public class BinaryTree
    {
        public BinaryTreeNode Root { get; private set; }

        public void Insert(string key, ViewState value)
        {
            Root = InsertRec(Root, key, value);
        }

        private BinaryTreeNode InsertRec(BinaryTreeNode node, string key, ViewState value)
        {
            if (node == null)
                return new BinaryTreeNode(key, value);
            if (string.Compare(key, node.Key) < 0)
                node.Left = InsertRec(node.Left, key, value);
            else
                node.Right = InsertRec(node.Right, key, value);
            return node;
        }
        public ViewState Search(string key)
        {
            return SearchRec(Root, key);
        }

        private ViewState SearchRec(BinaryTreeNode node, string key)
        {
            if (node == null) return null;
            int cmp = string.Compare(key, node.Key);
            if (cmp == 0)
                return node.Value;
            else if (cmp < 0)
                return SearchRec(node.Left, key);
            else
                return SearchRec(node.Right, key);
        }
    }
}
