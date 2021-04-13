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
            await Task.Delay((int)esperar * 10);

            return $"Hola, {nombre}!";
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
