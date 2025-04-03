using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fractales_ProyectoFinal
{
    class VentanaSierpinski : Form
    {
        //-----------------------------InterfazUsuario------------------------------
        int Ancho, Alto;
        Bitmap MapaPixeles;
        PictureBox plano;
        Button botonIniciar, botonLimpiar, botonDeshacer;
        Label etiquetaProfundidad;
        TextBox entradaProfundidad;
        Random aleatorio;
        Color colorTriangulo;
        Stack<Bitmap> historial = new Stack<Bitmap>();
        Queue<Color> colores = new Queue<Color>();
        Dictionary<int, int> contadorNiveles = new Dictionary<int, int>();
        ArbolFavoritos arbolFavoritos = new ArbolFavoritos();
        TextBox entradaBusqueda;
        Button botonGuardarFavorito, botonCargarFavorito;

        public VentanaSierpinski()
        {
            this.Text = "Triángulo de Sierpinski";
            this.Width = 900;
            this.Height = 700;
            Ancho = this.Width;
            Alto = this.Height;

            aleatorio = new Random();
            ConfigurarColores();

            // Configurar componentes
            ConfigurarInterfaz();
            ConfigurarInterfazFavoritos();
        }
        void ConfigurarInterfazFavoritos()
        {
            // Botón Guardar Favorito
            botonGuardarFavorito = new Button
            {
                Text = "Guardar Favorito",
                Size = new Size(100, 50),
                Location = new Point(750, 400),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            botonGuardarFavorito.Click += BotonGuardarFavorito_Click;

            // Botón Cargar Favorito
            botonCargarFavorito = new Button
            {
                Text = "Cargar Favorito",
                Size = new Size(100, 50),
                Location = new Point(750, 470),
                BackColor = Color.LightSalmon,
                FlatStyle = FlatStyle.Flat
            };
            botonCargarFavorito.Click += BotonCargarFavorito_Click;

            // Entrada para búsqueda
            entradaBusqueda = new TextBox
            {
                Location = new Point(750, 540),
                Size = new Size(100, 20),
                PlaceholderText = "Profundidad"
            };

            // Añadir componentes
            this.Controls.AddRange(new Control[] {
                botonGuardarFavorito, botonCargarFavorito, entradaBusqueda
            });
        }

        void ConfigurarColores()
        {
            colores.Enqueue(Color.Red);
            colores.Enqueue(Color.Blue);
            colores.Enqueue(Color.Green);
            colores.Enqueue(Color.Orange);
        }

        void ConfigurarInterfaz()
        {
            
            botonIniciar = new Button
            {
                Text = "Generar",
                Size = new Size(100, 50),
                Location = new Point(750, 100),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            botonIniciar.Click += BotonIniciar_Click;

            
            botonLimpiar = new Button
            {
                Text = "Borrar",
                Size = new Size(100, 50),
                Location = new Point(750, 170),
                BackColor = Color.IndianRed,
                FlatStyle = FlatStyle.Flat
            };
            botonLimpiar.Click += BotonLimpiar_Click;

            
            botonDeshacer = new Button
            {
                Text = "Deshace",
                Size = new Size(100, 50),
                Location = new Point(750, 240),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            botonDeshacer.Click += BotonDeshacer_Click;

            
            etiquetaProfundidad = new Label
            {
                Text = "Profundidad (0-8):",
                Location = new Point(750, 310)
            };

            entradaProfundidad = new TextBox
            {
                Location = new Point(750, 340),
                Size = new Size(100, 20),
                Text = "6"
            };

            // PictureBox
            plano = new PictureBox
            {
                Size = new Size(700, 700),
                Location = new Point(20, 20),
                BackColor = Color.White
            };

            // Añadir componentes
            this.Controls.AddRange(new Control[] {
                botonIniciar, botonLimpiar, botonDeshacer,
                etiquetaProfundidad, entradaProfundidad, plano
            });

            MapaPixeles = new Bitmap(plano.Width, plano.Height);
        }

        //-----------------------------------Metodos------------------------------------------------

        private void DibujarSierpinskiDFS(Graphics g, Point p1, Point p2, Point p3, int profundidad)
        {
            Stack<(Point, Point, Point, int)> pila = new Stack<(Point, Point, Point, int)>();
            pila.Push((p1, p2, p3, profundidad));
            contadorNiveles.Clear();

            using (Brush brocha = new SolidBrush(colorTriangulo))
            {
                while (pila.Count > 0)
                {
                    var (a, b, c, nivel) = pila.Pop();

                    if (nivel == 0)
                    {
                        Point[] puntos = { a, b, c };
                        g.FillPolygon(brocha, puntos);

                        // Contar por nivel
                        if (!contadorNiveles.ContainsKey(profundidad - nivel))
                            contadorNiveles[profundidad - nivel] = 0;
                        contadorNiveles[profundidad - nivel]++;
                    }
                    else
                    {
                        Point medio1 = PuntoMedio(a, b);
                        Point medio2 = PuntoMedio(b, c);
                        Point medio3 = PuntoMedio(c, a);

                        pila.Push((medio3, medio2, c, nivel - 1));
                        pila.Push((medio1, b, medio2, nivel - 1));
                        pila.Push((a, medio1, medio3, nivel - 1));
                    }
                }
            }
        }

        private Point PuntoMedio(Point a, Point b) => new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);

        //para guardar favoritos
        public class NodoArbol
        {
            public int Profundidad { get; set; }
            public Color Color { get; set; }
            public Bitmap Imagen { get; set; }
            public NodoArbol Izquierda { get; set; }
            public NodoArbol Derecha { get; set; }
        }

        public class ArbolFavoritos
        {
            public NodoArbol Raiz { get; set; }

            public void Insertar(NodoArbol nuevoNodo)
            {
                Raiz = InsertarRecursivo(Raiz, nuevoNodo);
            }

            private NodoArbol InsertarRecursivo(NodoArbol actual, NodoArbol nuevo)
            {
                if (actual == null) return nuevo;

                if (nuevo.Profundidad < actual.Profundidad)
                    actual.Izquierda = InsertarRecursivo(actual.Izquierda, nuevo);
                else
                    actual.Derecha = InsertarRecursivo(actual.Derecha, nuevo);

                return actual;
            }

            public NodoArbol Buscar(int profundidad)
            {
                return BuscarRecursivo(Raiz, profundidad);
            }

            private NodoArbol BuscarRecursivo(NodoArbol actual, int prof)
            {
                if (actual == null || actual.Profundidad == prof)
                    return actual;

                return prof < actual.Profundidad
                    ? BuscarRecursivo(actual.Izquierda, prof)
                    : BuscarRecursivo(actual.Derecha, prof);
            }
        }

        private void BotonIniciar_Click(object sender, EventArgs e)
        {            
            if (!int.TryParse(entradaProfundidad.Text, out int prof) || prof < 0 || prof > 8)
            {
                MessageBox.Show("Profundidad inválida. Use valores entre 0 y 8.");
                return;
            }
            historial.Push(new Bitmap(MapaPixeles));
            colorTriangulo = colores.Dequeue();
            colores.Enqueue(colorTriangulo);

            using (Graphics g = Graphics.FromImage(MapaPixeles))
            {
                g.Clear(Color.White);
                Point p1 = new Point(300, 50);
                Point p2 = new Point(50, 550);
                Point p3 = new Point(550, 550);
                DibujarSierpinskiDFS(g, p1, p2, p3, prof);
            }

            plano.Image = MapaPixeles;
            MostrarEstadisticas();
        }
        private void MostrarEstadisticas()
        {
            foreach (var nivel in contadorNiveles)
                Console.WriteLine($"Nivel {nivel.Key}: {nivel.Value} triángulos");
        }

        //----------------------BOTONES---------------------------

        private void BotonLimpiar_Click(object sender, EventArgs e)
        {
            MapaPixeles.Dispose();
            MapaPixeles = new Bitmap(plano.Width, plano.Height);
            plano.Image = MapaPixeles;
        }

        private void BotonDeshacer_Click(object sender, EventArgs e)
        {
            if (historial.Count > 0)
            {
                MapaPixeles.Dispose();
                MapaPixeles = historial.Pop();
                plano.Image = MapaPixeles;
            }
        }
        private void BotonGuardarFavorito_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(entradaProfundidad.Text, out int prof))
            {
                MessageBox.Show("Profundidad inválida");
                return;
            }

            arbolFavoritos.Insertar(new NodoArbol
            {
                Profundidad = prof,
                Color = colorTriangulo,
                Imagen = new Bitmap(MapaPixeles)
            });

            MessageBox.Show("Versión guardada en favoritos!");
        }

        private void BotonCargarFavorito_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(entradaBusqueda.Text, out int prof))
            {
                MessageBox.Show("Ingrese una profundidad válida");
                return;
            }

            NodoArbol favorito = arbolFavoritos.Buscar(prof);
            if (favorito == null)
            {
                MessageBox.Show("No se encontró esta versión");
                return;
            }

            MapaPixeles.Dispose();
            MapaPixeles = new Bitmap(favorito.Imagen);
            plano.Image = MapaPixeles;
            colorTriangulo = favorito.Color;
        }
    }
}