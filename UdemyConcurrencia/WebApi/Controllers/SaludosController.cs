using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Controllers.Helpers;

namespace WebApi.Controllers
{
    [Route("Saludos")]
    [ApiController]
    public class SaludosController : ControllerBase
    {
        //                              //Las comunicaciones HTTP
        //                              //Es una operacion IO.
        [HttpGet("{nombre}")]
        public ActionResult<String> ObtenerSaludos(String nombre)
        {
            return $"Hola, {nombre}!";
        }

        [HttpGet("delay/{nombre}")]
        public async Task<ActionResult<String>> ObtenerSaludoConDelay(String nombre)
        {
            var esperar = RandomGen.NextDouble() * 10 + 1;
            //await Task.Delay((int)esperar * 10);
            //OperacionVoidAsync();

            //                          //El try no evita que que se 
            //                          //  caiga la aplicaicon.
            //try
            //{
            //    //OperacionVoidAsync();
            //}
            //catch (Exception)
            //{
            //}

            //                          //Solucion 1.
            //OperacionTaskAsync();

            //                          //Solucion 2.
            //          |               //Utiliza un metodo sincrono.
            OperacionVoidSync();
            return $"Hola, {nombre}!";
        }

        private void OperacionVoidSync()
        {
            //                          //En este caso lanzamos una exception
            //                          //  en un metodo sincrono y eso no 
            //                          //  significa que el web api valla a 
            //                          //  colapsar.
            throw new ApplicationException();
        }

        //                          //Anti-patron: No debemos usar async void
        private async void OperacionVoidAsync()
        {
            await Task.Delay(3000);
            throw new ApplicationException();
        }

        private async Task OperacionTaskAsync()
        {
            await Task.Delay(3000);
            throw new ApplicationException();
        }

        [HttpGet("adios/{nombre}")]
        public async Task<ActionResult<String>> ObtenerAdiosConDelay(String nombre)
        {
            var esperar = RandomGen.NextDouble() * 10 + 1;
            await Task.Delay((int)esperar * 10);

            return $"bye, {nombre}!";
        }
    }
}
