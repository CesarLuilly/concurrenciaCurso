using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            var targetas = ObteneTargetasDeCredito(5);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                //        
                await ProcesarTargetas(targetas);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show($"Operacion finalizada en {stopwatch.ElapsedMilliseconds / 1000.0} segundos.");
            loadingGIF.Visible = false;
        }

        private async Task ProcesarTargetas(List<String> targetas)
        {
            var tareas = new List<Task>();

            foreach(var targeta in targetas)
            {
                var json = JsonConvert.SerializeObject(targeta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var respuestaTask = httpClient.PostAsync($"{strApiURL}/targetas", content);
                tareas.Add(respuestaTask);
            }

            await Task.WhenAll(tareas);
        }

        private List<String> ObteneTargetasDeCredito(
            int cantidadDeTargetas
            )
        {
            var targetas = new List<String>();
            for(int i =0; i < cantidadDeTargetas; i++)
            {
                //00000001
                targetas.Add(i.ToString().PadLeft(16, '0')); 
            }

            return targetas;
        }


        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(0));
        }

        private async Task<String> ObtenerSaludos(String nombre)
        {
            using (var respuesta =
                await httpClient.GetAsync($"{strApiURL}/saludos2/{nombre}"))
            {
                respuesta.EnsureSuccessStatusCode();
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
