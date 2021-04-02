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
        //                              //Este token es para cancelar
        //                              //  tareas y se declara a nivel de 
        //                              //  de clase.
        private CancellationTokenSource cancellationTokenSource;

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
            //                          //ESte token se les pasa a los metodos en donde
            //                          //  en donde yo quiero cancelar la tarea.
            cancellationTokenSource = new CancellationTokenSource();
            loadingGIF.Visible = true;
            pgProcesamiento.Visible = true;
            var resportarProgreso = new Progress<int>(ReportarProgresoTargetas);

            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var targetas = await ObteneTargetasDeCredito(20, 
                    cancellationTokenSource.Token);
                //        
                await ProcesarTargetas(targetas, resportarProgreso,
                    cancellationTokenSource.Token);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                //                      //Cuando la tarea es cancelada, lo que hace el token es
                //                      //  lanzar una excepcion.
                MessageBox.Show("La operacion ah sido cancelada.");
            }
            MessageBox.Show($"Operacion finalizada en {stopwatch.ElapsedMilliseconds / 1000.0} segundos.");
            loadingGIF.Visible = false;
            pgProcesamiento.Visible = false;
            pgProcesamiento.Value = 0;
        }

        private void ReportarProgresoTargetas(
            int porcentaje
            )
        {
            pgProcesamiento.Value = porcentaje;
        }

        private async Task ProcesarTargetas(List<String> targetas, 
            IProgress<int> progress = null, 
            CancellationToken cancellationToken = default)
        {
            //                          //Aqui decimos que vamor a realizar peticiones de 
            //                          //  2 en 2 con la finalidad de no abrumar a nuestro
            //                          //  servidor con tantas peticiones de golpe;
            using var semaforo = new SemaphoreSlim(2);
            var tareas = new List<Task<HttpResponseMessage>>();
            int indice = 0;
            tareas = targetas.Select(async targeta =>
                {
                    var json = JsonConvert.SerializeObject(targeta);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //                  //Lo que hace el semaforo es procesar 400 tareas,
                    //                  //  y va a continuar cuando se hallan procesado
                    //                  //  las 4000 tareas, en este caso.
                    await semaforo.WaitAsync();
                    //                  //Libero el samaforo y llegara a encontrar algun error.
                    try
                    {
                        var tareaInterna = await httpClient.PostAsync($"{strApiURL}/targetas", content,
                            cancellationToken);
                        //if (
                        //    progress != null
                        //)
                        //{
                        //    indice++;
                        //    var porcentaje = (double)indice / targetas.Count;
                        //    porcentaje = porcentaje * 100;
                        //    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                        //    progress.Report(porcentajeInt);
                        //}
                        return tareaInterna;
                    }
                    finally
                    {
                        //              //Libero los recursos de semaforo.
                        semaforo.Release();
                    }

                }).ToList();

            //                          //Con await podemos obtener el resultado de esas tareas y 
            //                          //  luego procesar el resultado de esas tareas, en este caso, 
            //                          //  de las peticiones que se icieron vamos a ver que targetas
            //                          //  fueron rechazadas.
            var respuestasTareas = Task.WhenAll(tareas);

            if (
                progress != null)
            {
                //                      //Re
                while (
                    //                  //TaskWhenAny va a retornar la tarea que culmino.
                    //                  //Es decir, tomando el task.delay, esa tarea termina en cada 
                    //                  //  segundo, per se va a salir del ciclo hasta que la tarea que 
                    //                  //  haya terminado sea la de respuestasTareas.
                    //                  //ESTA TECNICA ES LA MAS VIABLE Y NOS PERMITE EJECUTAR UNA PIEZA 
                    //                  //  DE CODIGO, EN ESTE CASO UN PEDAZO DE CODIGO
                    //                  //  POR CADA SEGUNDO.
                    await Task.WhenAny(respuestasTareas, Task.Delay(1000)) 
                    != respuestasTareas)
                {
                    var tareasCompletadas = tareas.Where(x => x.IsCompleted).Count();
                    var porcentaje = (double)tareasCompletadas / targetas.Count;
                    porcentaje = porcentaje * 100;
                    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                    progress.Report(porcentajeInt);

                }
            }

            //                          //NOTA. SI LA TAREA YA AH SIDO COMPLETADA, 
            //                          //  Y SI LE LLEGO A PONER UN AWAIT A LA TAREA, 
            //                          //  NO QUIERE DECIR QUE LA TAREA SE VA EJECUTAR
            //                          //  2 VESES, SIMPLEMENTE SIGUE LA EJECUCION.
            var respuestas = await respuestasTareas;

            var tagetasRechazadas = new List<String>();
            foreach (var respuesta in respuestas)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var respuestaTargeta = JsonConvert
                    .DeserializeObject<RespuestaTargeta>(contenido);
                if (
                    !respuestaTargeta.Aprobada
                    )
                {
                    tagetasRechazadas.Add(respuestaTargeta.Targeta);
                }
            }
            foreach (var targeta in tagetasRechazadas)
            {
                Console.WriteLine(targeta);
            }
        }

        private async Task<List<String>>  ObteneTargetasDeCredito(
            int cantidadDeTargetas, 
            //                          //Con default lo que decimos es que 
            //                          //  el cancelation Token no es obligatorio
            //                          //  pasarlo al metodo.
            CancellationToken cancellationToken = default
            )
        {
            //                          //Encerrando el ciclo en un task,
            //                          //  de esta forma liberamos el hilo UI
            //                          //  si es se llegaran a procesar muchas targetas.
            return await Task.Run(async () => 
            {
                var targetas = new List<String>();
                for (int i = 0; i < cantidadDeTargetas; i++)
                {
                    //                  //con esto vamos a simular que vamos a
                    //                  //  esta haciendo un procesamiento largo.
                    await Task.Delay(1000);
                    //00000001
                    targetas.Add(i.ToString().PadLeft(16, '0'));

                    Console.WriteLine($"Han sido generadas  { targetas.Count } ");
                    if (
                        //              //Lo que me dise es ver si el token ya ha 
                        //              //  solicitado la cancelacion del token.
                        cancellationToken.IsCancellationRequested
                    )
                    {
                        throw new TaskCanceledException();
                    }
                }

                return targetas;
            });
            
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //                          //Esto lo que hace es lanzar una excepcion
            //                          //  con la finalidad de cancelar la exception.
            //                          //Para eso nosotros necesitamos atrapar la
            //                          //  excepcion.
            cancellationTokenSource?.Cancel();
        }
    }
}
