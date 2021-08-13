using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Domain.Models;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ProEventosContext context;
        public EventoController(ProEventosContext context)
        {
            this.context = context;

        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return context.Eventos.Where(x=>x.Id==id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return context.Eventos;
        }
        
        [HttpPost]
        public IEnumerable<Evento> Post([FromBodyAttribute] Evento evento)
        {
            return context.Eventos;
        }
    }
}
