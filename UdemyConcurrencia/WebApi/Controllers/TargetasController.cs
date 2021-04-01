using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.Helpers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("targetas")]
    public class TargetasController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> procesarTargetas([FromBody] string targeta)
        {
            var valorAleatorio = RandomGen.NextDouble();
            var aprobada = valorAleatorio > 0.1;
            await Task.Delay(1000);
            Console.WriteLine($"Targeta {targeta} procesada");
            return Ok(new { targeta = targeta, Aprobada = aprobada });

        }
    }
}
