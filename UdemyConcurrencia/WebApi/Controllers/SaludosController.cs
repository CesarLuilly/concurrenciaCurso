using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
