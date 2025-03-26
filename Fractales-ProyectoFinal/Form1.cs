namespace Fractales_ProyectoFinal
{
    public partial class Form1 : Form
    {
        int count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count++;
            label1.Text = count.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
