using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdemyConcurrencia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            loadingGIF.Visible = true;
            //                          //Si no utilizamos await, el hilo no se va a detener y va
            //                          //  a seguir ejecutando las siguientes lineas de codigo sin
            //                          //  importar lo que suceda con el metodo espera().
            //                          //En ocaciones eso puede estar bien, pero todo depende de la 
            //                          //  solucion del problema.
            //                          //Con await, lo que queremos decir es, suspende la ejecucion
            //                          //  de esta tarea(metodo esperar) y cuando termine la tarea, 
            //                          //  entonces continua con las siguientes lineas de codigo.
            await Esperar();
            MessageBox.Show("pasaron los 5 segundos");
            loadingGIF.Visible = false;
        }

        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
