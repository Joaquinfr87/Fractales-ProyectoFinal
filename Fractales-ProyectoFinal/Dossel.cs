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
        int Ancho, Alto;
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
            longitud = longitud * 0.5;
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

    class Ventana : Form
    {
        int Ancho, Alto;
        Bitmap Mapa_pixeles;
        Arbol arbol;
        public Ventana()
        {
            this.Text = "Fractales";
            this.Width = 1000;
            this.Height = 1000;
            Ancho = this.Width;
            Alto = this.Height;

            arbol = new Arbol(Ancho, Alto);
            arbol.LlenadoArbol(arbol.raiz, 500.0, 45, 90, 20);

            PictureBox plano = new PictureBox();
            plano.Size = new Size(Ancho, Alto);
            plano.BackColor = Color.Black;

            Mapa_pixeles = new Bitmap(Ancho, Alto);

            this.Controls.Add(plano);

            plano.Image = Mapa_pixeles;

            Dibujar(Graphics.FromImage(Mapa_pixeles), new Pen(Color.Blue, 1), arbol.raiz);


        }
        public void Dibujar(Graphics g, Pen lapiz, Nodo nodo)
        {
            if (nodo == arbol.raiz) { g.DrawLine(lapiz, 500, 1000, 500 - (float)nodo.x, 1000 - (float)nodo.y); }
            if (nodo.Izquierda == null || nodo.Derecha == null) { return; }
            g.DrawLine(lapiz, 500 - (float)nodo.x, 1000 - (float)nodo.y, 500 - (float)nodo.Izquierda.x, 1000 - (float)nodo.Izquierda.y);
            g.DrawLine(lapiz, 500 - (float)nodo.x, 1000 - (float)nodo.y, 500 - (float)nodo.Derecha.x, 1000 - (float)nodo.Derecha.y);


            Dibujar(g, lapiz, nodo.Izquierda);
            Dibujar(g, lapiz, nodo.Derecha);

        }

    }
}
