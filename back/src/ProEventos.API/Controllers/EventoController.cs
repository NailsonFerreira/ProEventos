using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Data;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly DataContext context;

        public EventoController(DataContext context)
        {
            this.context = context;

        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return context.Eventos.Where(x=>x.EventoId==id).FirstOrDefault();
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
