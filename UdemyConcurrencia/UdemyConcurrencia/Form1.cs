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
            //                          //Delay me permite crear una tarea de lo
            //                          //  cual va a expirar en x segundos.
            await Task.Delay(TimeSpan.FromSeconds(5));
            //                          //Con await lo que hacemos es liberar el hilo,
            //                          //  para que no se bloquee la aplicacion, y 
            //                          //  seguir intercatuando con la pagina.
            //                          // .............
            //                          //Y ya cuando la tarea se termine, se van a seguir
            //                          //  ejecutando las siguientes lineas de codigo 
            //                          //  el mismo hilo UI que inicio esta operacion.
            loadingGIF.Visible = false;
        }
    }
}
