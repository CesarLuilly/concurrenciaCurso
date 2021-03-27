using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdemyConcurrencia
{
    public partial class Form1 : Form
    {
        private string strApiURL;
        private HttpClient httpClient;
        public Form1()
        {
            InitializeComponent();
            strApiURL = "https://localhost:44336";
            httpClient = new HttpClient();
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
            var nombre = txtInput.Text;
            var saludo = await ObtenerSaludos(nombre);
            //                          //Si no utilizamos await, el hilo no se va a detener y va
            //                          //  a seguir ejecutando las siguientes lineas de codigo sin
            //                          //  importar lo que suceda con el metodo espera().
            //                          //En ocaciones eso puede estar bien, pero todo depende de la 
            //                          //  solucion del problema.
            //                          //Con await, lo que queremos decir es, suspende la ejecucion
            //                          //  de esta tarea(metodo esperar) y cuando termine la tarea, 
            //                          //  entonces continua con las siguientes lineas de codigo.
            await Esperar();
            MessageBox.Show(saludo);
            loadingGIF.Visible = false;
        }

        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        private async Task<String> ObtenerSaludos(String nombre)
        {
            using (var respuesta =
                await httpClient.GetAsync($"{strApiURL}/saludos/{nombre}"))
            {
                //                      //Aqui voy a leer el contenido de la 
                //                      //  respuesta http.
                var saludo = await respuesta.Content.ReadAsStringAsync();
                return saludo;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
