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

        private async void btnIniciar_Click(object sender, EventArgs e) {
            loadingGIF.Visible = true;
            //var nombres = new List<String> { "Cesar", "Garcia" };
            //foreach (var nombre in nombres)
            //{
            //}

            //                          //Lo que hace aqui es que va al metodo, 
            //                          //  el primer valor de yeld que encuentra, 
            //                          //En la segunda iteracion lo que hace es ir 
            //                          //  metodo y busca el segundo valor de yeld.
            foreach (var nombre in GenerarNombres())
            { Console.WriteLine(nombre); }

            //                          //AsyncEnumerable es la version asincrona de 
            //                          //  IEnumerable por lo tanto nos permite realizar 
            //                          //  iteraciones desde operaciones asincronas.
            //                          //Un lugar donde es importante utilizar esto, es 
            //                          //  cuando estamos obteniendo los valores de un 
            //                          //  servicio WEB de mannera que, en el metodo que 
            //                          //  devuelve el iterable, estamos realizando 
            //                          //  peticiones HTTP las cuales gradualmente nos 
            //                          //  van devolviendo valores de webservices el cual
            //                          //  nos permite sencillamente obtener todos los 
            //                          //  valores en una sola peticion HTTP, si no que 
            //                          //  nos exije que recorramos una paginacion.
            await foreach (var nombre in GenerarNombresAsync())
            { Console.WriteLine(nombre); }

            loadingGIF.Visible = false;
        }

        private async IAsyncEnumerable<String> GenerarNombresAsync(
            //                          /AsyncEnumerable es la version asincrona de IEnumerable.

            )
        {

            //                          //Aqui lo que estamos haciendo es creando
            //                          //  un IEnumerable, es decir que estamos creando
            //                          //  tipo que es iterable, y eso significa que 
            //                          //  yo puedo iterar este metodo generar nomres, es
            //                          //  decir, el resultado del metodo generar nombres.

            //                          //Aqui le estoy especificando como el primer elemento 
            //                          //  de la lista.
            yield return "CesarAsync";

            await Task.Delay(3000);
            //                          //Aqui le estoy especificando como el segudo elemento 
            //                          //  de la lista.
            yield return "GarciaAsync";
        }

        private IEnumerable<String> GenerarNombres(
            //                          //Metod sincrono que devuelve el IEnumerable.
            
            ) {

            //                          //Aqui lo que estamos haciendo es creando
            //                          //  un IEnumerable, es decir que estamos creando
            //                          //  tipo que es iterable, y eso significa que 
            //                          //  yo puedo iterar este metodo generar nomres, es
            //                          //  decir, el resultado del metodo generar nombres.

            //                          //Aqui le estoy especificando como el primer elemento 
            //                          //  de la lista.
            yield return "Cesar";

            //                          //Aqui le estoy especificando como el segudo elemento 
            //                          //  de la lista.
            yield return "Garcia";
        }


        public Task EvaluarValor(
            //                          //Metodo que retorna una tarea.
            String valor
            )
        {
            var tcs = new TaskCompletionSource<object>
                (TaskCreationOptions.RunContinuationsAsynchronously);
            if (
                valor == "1"
                )
            {
                tcs.SetResult(null);
            }
            else if (
                valor =="2"               
                )
            {
                tcs.SetCanceled();
            }
            else
            {
                tcs.SetException(new ApplicationException($"Valor invalidad: {valor}"));
            }
            return tcs.Task;
        }

        private async Task<T> EjecutarUno<T>(
            //                          //Tenemos un enumerable, es decir una 
            //                          //  coleccion iterable de funciones las
            //                          //  cuales van a recibir como parametro un
            //                          //  CancellationToken y van a devolver un 
            //                          //  Task<T>, y a esta colecion de funciones,
            //                          //  les estamos llamando funciones.
            IEnumerable<Func<CancellationToken, Task<T>>> funciones
            )
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            //                          //WhenAny devuelve la primer tarea que se
            //                          //  termina primero de un conjunto de tareas,
            //                          //  cuando se haya devuelto la tarea continua
            //                          //  con la ejecucion.
            var tarea = await Task.WhenAny(tareas);
            //                          //Con la siguiente instruccion estoy cancelando
            //                          //  las demas tareas.
            cts.Cancel();
            return await tarea;
        }

        private async Task<T> EjecutarUnoV2<T>(
            params 

            //                          //Con paramas estamos diciendo que podemos 
            //                          //  enviar tantos parametros que sean del 
            //                          //  tipo de dato especificado
            Func<CancellationToken, Task<T>>[] funciones
            )
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            //                          //WhenAny devuelve la primer tarea que se
            //                          //  termina primero de un conjunto de tareas,
            //                          //  cuando se haya devuelto la tarea continua
            //                          //  con la ejecucion.
            var tarea = await Task.WhenAny(tareas);
            //                          //Con la siguiente instruccion estoy cancelando
            //                          //  las demas tareas.
            cts.Cancel();
            return await tarea;
        }

        private async Task<String> ObtenerSaludos(
            String nombre,
            CancellationToken cancellationToken
            )
        {
            using (var respuesta =
                await httpClient.GetAsync($"{strApiURL}/saludos/delay/{nombre}",
                cancellationToken))
            {
                respuesta.EnsureSuccessStatusCode();
                //                      //Aqui voy a leer el contenido de la 
                //                      //  respuesta http.
                var saludo = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(saludo);
                return saludo;
            }
        }

        private async Task<String> ObtenerAdios(
            String nombre,
            CancellationToken cancellationToken
            )
        {
            using (var respuesta =
                await httpClient.GetAsync($"{strApiURL}/saludos/adios/{nombre}",
                cancellationToken))
            {
                respuesta.EnsureSuccessStatusCode();
                //                      //Aqui voy a leer el contenido de la 
                //                      //  respuesta http.
                var saludo = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(saludo);
                return saludo;
            }
        }

        private async Task ProcesarSaludos()
        {
            using (var respuesta = await httpClient.GetAsync($"{strApiURL}/Saludos/Cesar"))
            {
                //              //Con la siguiente linea, indicamos que 
                //              //  va a lanzar una excepcion en caso de la
                //              //  respuesta no sea sastifactoria.
                respuesta.EnsureSuccessStatusCode();
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
            }
        }

        private async Task Reintentar(
            //                          //Funcion es el que realiza la peticion HTTP.
            Func<Task> f, 
            int reintentos = 3, 
            int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos; i++)
            {
                try
                {
                    //                  //Operacion, Peticion HTTP.
                    await f();
                    //                  //Uso de brake para salir del for,
                    //                  //  cuando la peticion haya sido
                    //                  //  exitosa.
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }
        }

        private async Task<T> Reintentar<T>(
            //                          //Funcion es el que realiza la peticion HTTP.
            Func<Task<T>> f,
            int reintentos = 3,
            int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos - 1 /*vamos a realizar 2 peticion*/; i++)
            {
                try
                {
                    //                  //Operacion, Peticion HTTP.
                    return await f();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }

            //                          //Si los reintentos anteriores fallan, aqui 
            //                          //  se va a realizar otro intento.
            //                          //Si llegara a haber una exception aqui, 
            //                          //  esa exception va a subir hacia el cliente
            //                          //  de este metodo(Reintentar();)
            return await f();
        }

        private void ReportarProgresoTargetas(
            int porcentaje
            )
        {
            pgProcesamiento.Value = porcentaje;
        }
        private Task ProcesarTargetasMock(List<String> targetas,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            //                          //Lo que hace es que se crea una tarea,
            //                          //  lo cual por defecto ya se encuentra 
            //                          //  completada y lo retornamos, .
            return Task.CompletedTask;
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

        private Task<List<String>> ObteneTargetasDeCreditoMock(
            int cantidadDeTargetas,
            CancellationToken cancellationToken = default
            )
        { 
            
            var targetas = new List<String>();
            targetas.Add("0000000001");

            //                          //En el siguiente codigo estamos 
            //                          //  estas targetas dentro de una tarea lo cual
            //                          //  ya se encuentra sastifactoriamente
            //                          //  completada.
            return Task.FromResult(targetas);
        }

        private Task obtenerTareaConError()
        {

            //                          //De esta manera esta tarea se ah configurado
            //                          //  con un error. 
            //                          //Estos es importante en prubas unitarias para
            //                          //  comproboar que el codigo responde correctamente 
            //                          //  cuando un metodo retorna una tarea con un error.
            //                          //De esta manera podemos estar seguros que nuestro
            //                          //  software procesa esto de manera correcta.
            return Task.FromException(new ApplicationException());
        }

        private Task obtenerTareasCanceladas()
        {
            //                          //Con esto estamos obteniendo una tarea que ah sido
            //                          //  cancelada, 
            //                          //Esto es util, sobre todo cuando estamos haciendo
            //                          //  pruebas unitarias y queremos ver que nuestro
            //                          //  software responde bien a tareas que han sido 
            //                          //  canceladas.
            cancellationTokenSource = new CancellationTokenSource();
            return Task.FromCanceled(cancellationTokenSource.Token);
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
            return await Task.Run(/*async*/ () => 
            {
                var targetas = new List<String>();
                for (int i = 0; i < cantidadDeTargetas; i++)
                {
                    //                  //con esto vamos a simular que vamos a
                    //                  //  esta haciendo un procesamiento largo.
                    //await Task.Delay(1000);
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
