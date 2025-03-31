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

        public void LlenadoArbol(Nodo nodo, double longitud, int angulo, int angulo_inicial, int profundidad)
        {
            if (profundidad == 0) return;

            if (nodo == null)
            {
                double x = Math.Cos(angulo_inicial * Math.PI / 180) * longitud;
                double y = Math.Sin(angulo_inicial * Math.PI / 180) * longitud;
                nodo = new Nodo(x, longitud);
                raiz = nodo;
            }
            longitud = longitud*0.6;
            double nuevaXIzq = nodo.x + Math.Cos((angulo_inicial + angulo) * Math.PI / 180) * longitud;
            double nuevaYIzq = nodo.y + Math.Sin((angulo_inicial + angulo) * Math.PI / 180) * longitud;
            double nuevaXDer = nodo.x + Math.Cos((angulo_inicial - angulo) * Math.PI / 180) * longitud;
            double nuevaYDer = nodo.y + Math.Sin((angulo_inicial - angulo) * Math.PI / 180) * longitud;
            
            nodo.Izquierda = new Nodo(nuevaXIzq, nuevaYIzq);
            LlenadoArbol(nodo.Izquierda, longitud, angulo, angulo_inicial + angulo, profundidad - 1);
           
            
            nodo.Derecha = new Nodo(nuevaXDer, nuevaYDer);
            LlenadoArbol(nodo.Derecha, longitud, angulo, angulo_inicial - angulo, profundidad - 1);
        }
    }

    class VentanaDossel : Form
    {
        int Ancho, Alto;
        Bitmap Mapa_pixeles;
        Arbol arbol;
        Button botonIniciar;
        PictureBox plano;

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

            plano = new PictureBox();
            plano.Size = new Size(Ancho-200, Alto);
            plano.BackColor = Color.Black;

            Mapa_pixeles = new Bitmap(Ancho-200, Alto);

            this.Controls.Add(plano);
            this.Controls.Add(botonIniciar);
            plano.Image = Mapa_pixeles;


            

        }
        public void Dibujar(Graphics g, Pen lapiz, Nodo nodo)
        {
            if (nodo == arbol.raiz) { g.DrawLine(lapiz, arbol.Ancho/2, arbol.Alto, arbol.Ancho/2 - (float)nodo.x, arbol.Alto - (float)nodo.y); }
            if (nodo.Izquierda == null || nodo.Derecha == null) { return; }
            g.DrawLine(lapiz, arbol.Ancho/2 - (float)nodo.x, arbol.Alto- (float)nodo.y, arbol.Ancho/2- (float)nodo.Izquierda.x, arbol.Alto- (float)nodo.Izquierda.y);
            g.DrawLine(lapiz, arbol.Ancho/ 2 - (float)nodo.x, arbol.Alto - (float)nodo.y, arbol.Ancho/2 - (float)nodo.Derecha.x, arbol.Alto - (float)nodo.Derecha.y);


            Dibujar(g, lapiz, nodo.Izquierda);
            Dibujar(g, lapiz, nodo.Derecha);
        }
        private void botonIniciar_Click(object sender, EventArgs e)
        {
            arbol = new Arbol(Ancho - 200, Alto);
            arbol.LlenadoArbol(arbol.raiz, 200, 45, 90, 10);
            
            Dibujar(Graphics.FromImage(Mapa_pixeles), new Pen(Color.Blue, 1), arbol.raiz);
            
            plano.Image = Mapa_pixeles;
            plano.Refresh();

        }

    }
}
