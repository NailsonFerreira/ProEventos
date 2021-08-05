using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {

        public EventoController()
        {
            
        }

        [HttpGet]
        public Evento Get()
        {
            return new Evento{
                EventoId=0,
                Tema= "Xuxa",
                DataEvento=DateTime.Now.ToString(),
                Local= "Piedade",
                QuantidadePessoas=250,
                Lote="36n",
                ImagemUrl= "www.google.com"
            };
        }
    }
}
