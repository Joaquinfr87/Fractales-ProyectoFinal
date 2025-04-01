using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractales_ProyectoFinal
{
    class Nodo
    {
        public Nodo Izquierda, Derecha;
        public double x, y;
        public Nodo(double X, double Y) { x = X; y = Y; }
    }
    class Arbol
    {

        public Nodo raiz;
        public int Ancho, Alto;
        Random random = new Random();
        public Arbol(int ancho, int alto) { Ancho = ancho; Alto = alto; }

            public void LlenadoArbol(Nodo nodo, double longitud, int angulo, int angulo_inicial, int profundidad,double razon)
            {
            
                
                if (profundidad == 0) return;

                if (nodo == null)
                {
                    double x = Math.Cos(angulo_inicial * Math.PI / 180) * longitud;
                    double y = Math.Sin(angulo_inicial * Math.PI / 180) * longitud;
                    nodo = new Nodo(x, y);
                    raiz = nodo;
                }
                longitud = longitud*razon;
                double nuevaXIzq = nodo.x + Math.Cos((angulo_inicial + angulo) * Math.PI / 180) * longitud;
                double nuevaYIzq = nodo.y + Math.Sin((angulo_inicial + angulo) * Math.PI / 180) * longitud;
                double nuevaXDer = nodo.x + Math.Cos((angulo_inicial - angulo) * Math.PI / 180) * longitud;
                double nuevaYDer = nodo.y + Math.Sin((angulo_inicial - angulo) * Math.PI / 180) * longitud;
            
                nodo.Izquierda = new Nodo(nuevaXIzq, nuevaYIzq);
                LlenadoArbol(nodo.Izquierda, longitud, angulo, angulo_inicial + angulo, profundidad - 1,razon);
           
            
                nodo.Derecha = new Nodo(nuevaXDer, nuevaYDer);
                LlenadoArbol(nodo.Derecha, longitud, angulo, angulo_inicial - angulo, profundidad - 1,razon);
            }
    }

    class VentanaDossel : Form
    {
        int Ancho, Alto;
        Bitmap Mapa_pixeles;
        PictureBox plano;
        
        Arbol arbol;

        Button botonIniciar;
        Button botonLimpiar;

        Label Longitud_label;
        TextBox Longitud_input;
        Label AnguloInicial_label;   
        TextBox AnguloInicial_input;
        Label Angulo_label;
        TextBox Angulo_input;
        Label Profundidad_label;
        TextBox Profundidad_input;
        Label Razon_label;
        TextBox Razon_input;
        Label Color_label;
        TextBox Color_input;
        Label origenX_label;
        TextBox origenX_input;
        Label origenY_label;
        TextBox origenY_input;
        public VentanaDossel()
        {

            this.Text = "Fractales";
            this.Width = 1100;
            this.Height = 700;
            Ancho = this.Width;
            Alto = this.Height;

            botonIniciar = new Button();
            botonIniciar.Text = "Iniciar";
            botonIniciar.Size = new Size(100, 50);
            botonIniciar.Location = new Point(Width-150, 5);
            botonIniciar.Click+=new EventHandler(botonIniciar_Click);
            this.Controls.Add(botonIniciar);

            botonLimpiar = new Button();
            botonLimpiar.Text = "Limpiar";
            botonLimpiar.Size = new Size(100, 50);
            botonLimpiar.Location = new Point(Width - 150, 60);
            botonLimpiar.Click += new EventHandler(botonLimpiar_Click);
            this.Controls.Add(botonLimpiar);

            Longitud_label = new Label();
            Longitud_label.Text = "Longitud";
            Longitud_label.Location = new Point(Width-135, 150);
            this.Controls.Add(Longitud_label);

            Longitud_input = new TextBox();
            Longitud_input.Location = new Point(Width - 150, 175);
            Longitud_input.Text = "250.0";
            this.Controls.Add(Longitud_input);

            AnguloInicial_label = new Label();
            AnguloInicial_label.Text = "Angulo Inicial";
            AnguloInicial_label.Location = new Point(Width - 135, 200);
            this.Controls.Add(AnguloInicial_label);

            AnguloInicial_input = new TextBox();
            AnguloInicial_input.Location = new Point(Width - 150, 225);
            AnguloInicial_input.Text = "90";
            this.Controls.Add(AnguloInicial_input);

            Angulo_label = new Label();
            Angulo_label.Text = "Angulo";
            Angulo_label.Location = new Point(Width - 135, 250);
            this.Controls.Add(Angulo_label);
            
            Angulo_input = new TextBox();
            Angulo_input.Location = new Point(Width - 150, 275);
            Angulo_input.Text = "45";
            this.Controls.Add(Angulo_input);

            Profundidad_label = new Label();
            Profundidad_label.Text = "Profundidad";
            Profundidad_label.Location = new Point(Width - 135, 300);
            this.Controls.Add(Profundidad_label);

            Profundidad_input = new TextBox();
            Profundidad_input.Location = new Point(Width - 150, 325);
            Profundidad_input.Text = "10";
            this.Controls.Add(Profundidad_input);
           
            Razon_label = new Label();
            Razon_label.Text = "Razon";
            Razon_label.Location = new Point(Width - 135, 350);
            this.Controls.Add(Razon_label);

            Razon_input = new TextBox();
            Razon_input.Location = new Point(Width - 150, 375);
            Razon_input.Text = "0.6";
            this.Controls.Add(Razon_input);

            Color_label = new Label();
            Color_label.Text = "Color";
            Color_label.Location = new Point(Width - 135, 400);
            this.Controls.Add(Color_label);

            Color_input = new TextBox();
            Color_input.Location = new Point(Width - 150, 425);
            Color_input.Text = "Blue";
            this.Controls.Add(Color_input);

            origenX_label = new Label();
            origenX_label.Text = "Origen X";
            origenX_label.Location = new Point(Width - 135, 450);
            this.Controls.Add(origenX_label);

            origenX_input = new TextBox();
            origenX_input.Location = new Point(Width - 150, 475);
            origenX_input.Text = $"{(Ancho-200)/2}";
            this.Controls.Add(origenX_input);

            origenY_label = new Label();
            origenY_label.Text = "Origen Y";
            origenY_label.Location = new Point(Width - 135, 500);
            this.Controls.Add(origenY_label);

            origenY_input = new TextBox();
            origenY_input.Location = new Point(Width - 150, 525);
            origenY_input.Text = $"{Alto}";
            this.Controls.Add(origenY_input);

            plano = new PictureBox();
            plano.Size = new Size(Ancho-200, Alto);
            plano.BackColor = Color.Black;
            this.Controls.Add(plano);

            Mapa_pixeles = new Bitmap(Ancho-200, Alto);



        }
            public void Dibujar(Graphics g, Pen lapiz, Nodo nodo,int origenX,int origenY)
            {
                if (nodo == arbol.raiz) {g.DrawLine(lapiz, origenX, origenY, origenX- (float)nodo.x, origenY - (float)nodo.y); }
                if (nodo.Izquierda == null || nodo.Derecha == null) { return; }
                g.DrawLine(lapiz, origenX - (float)nodo.x, origenY- (float)nodo.y, origenX- (float)nodo.Izquierda.x, origenY- (float)nodo.Izquierda.y);
                g.DrawLine(lapiz, origenX - (float)nodo.x, origenY- (float)nodo.y, origenX- (float)nodo.Derecha.x, origenY - (float)nodo.Derecha.y);

                Dibujar(g, lapiz, nodo.Izquierda,origenX,origenY);
                Dibujar(g, lapiz, nodo.Derecha,origenX,origenY);
            }
            private void botonIniciar_Click(object sender, EventArgs e)
            {
                double longitud = double.Parse(Longitud_input.Text);
                int angulo = int.Parse(Angulo_input.Text);
                int angulo_inicial = int.Parse(AnguloInicial_input.Text);
                int profundidad = int.Parse(Profundidad_input.Text);
                double razon = double.Parse(Razon_input.Text);

                arbol = new Arbol(Ancho - 200, Alto);
                arbol.LlenadoArbol(arbol.raiz, longitud, angulo, angulo_inicial, profundidad,razon);
                
                int origenX=int.Parse(origenX_input.Text);
                int origenY = int.Parse(origenY_input.Text);
            
                Dibujar(Graphics.FromImage(Mapa_pixeles), new Pen(Color.FromName(Color_input.Text), 1), arbol.raiz,origenX,origenY);
            
                plano.Image = Mapa_pixeles;
                plano.Refresh();

            }
            private void botonLimpiar_Click(object sender, EventArgs e) {
                Mapa_pixeles.Dispose();
                Mapa_pixeles= new Bitmap(Ancho - 200, Alto);
                plano.Image = Mapa_pixeles;
                plano.Refresh();
        }

    }
}
