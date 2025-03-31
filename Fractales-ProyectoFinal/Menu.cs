using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractales_ProyectoFinal
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VentanaDossel fractal1 = new VentanaDossel();
            fractal1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fractal2 = new Form1();
            fractal2.Show();
        }
    }
}
