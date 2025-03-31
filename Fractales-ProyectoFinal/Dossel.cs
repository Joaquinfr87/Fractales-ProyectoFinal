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
        public Arbol(int ancho, int alto) { Ancho = ancho; Alto = alto; }

        public void LlenadoArbol(Nodo nodo, double longitud, int angulo, int angulo_inicial, int profundidad,double razon)
        {
            if (profundidad == 0) return;

            if (nodo == null)
            {
                double x = Math.Cos(angulo_inicial * Math.PI / 180) * longitud;
                double y = Math.Sin(angulo_inicial * Math.PI / 180) * longitud;
                nodo = new Nodo(x, longitud);
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
            botonIniciar.Location = new Point(Width-150, 50);
            botonIniciar.Click+=new EventHandler(botonIniciar_Click);
            this.Controls.Add(botonIniciar);

            Longitud_label = new Label();
            Longitud_label.Text = "Longitud";
            Longitud_label.Location = new Point(Width-135, 150);
            this.Controls.Add(Longitud_label);

            Longitud_input = new TextBox();
            Longitud_input.Location = new Point(Width - 150, 175);
            Longitud_input.Text = "200.0";
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




            plano = new PictureBox();
            plano.Size = new Size(Ancho-200, Alto);
            plano.BackColor = Color.Black;
            this.Controls.Add(plano);

            Mapa_pixeles = new Bitmap(Ancho-200, Alto);



        }
        public void Dibujar(Graphics g, Pen lapiz, Nodo nodo)
        {
            if (nodo == arbol.raiz) {g.DrawLine(lapiz, arbol.Ancho/2, arbol.Alto, arbol.Ancho/2 - (float)nodo.x, arbol.Alto - (float)nodo.y); }
            if (nodo.Izquierda == null || nodo.Derecha == null) { return; }
            g.DrawLine(lapiz, arbol.Ancho/2 - (float)nodo.x, arbol.Alto- (float)nodo.y, arbol.Ancho/2- (float)nodo.Izquierda.x, arbol.Alto- (float)nodo.Izquierda.y);
            g.DrawLine(lapiz, arbol.Ancho/ 2 - (float)nodo.x, arbol.Alto - (float)nodo.y, arbol.Ancho/2 - (float)nodo.Derecha.x, arbol.Alto - (float)nodo.Derecha.y);

            Dibujar(g, lapiz, nodo.Izquierda);
            Dibujar(g, lapiz, nodo.Derecha);
        }
        private void botonIniciar_Click(object sender, EventArgs e)
        {
            double longitud = double.Parse(Longitud_input.Text);
            int angulo = int.Parse(Angulo_input.Text);
            int angulo_inicial = int.Parse(AnguloInicial_input.Text);
            int profundidad = int.Parse(Profundidad_input.Text);
            double razon = double.Parse(Razon_input.Text);

            arbol = new Arbol(Ancho - 200, Alto);
            //arbol.LlenadoArbol(arbol.raiz, 200.0, 45, 90, 10, 0.6);
            arbol.LlenadoArbol(arbol.raiz, longitud, angulo, angulo_inicial, profundidad,razon);

            
            Dibujar(Graphics.FromImage(Mapa_pixeles), new Pen(Color.Blue, 1), arbol.raiz);
            
            plano.Image = Mapa_pixeles;
            plano.Refresh();

        }

    }
}
