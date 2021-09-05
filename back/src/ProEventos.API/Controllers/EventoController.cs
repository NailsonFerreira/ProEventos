using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService service;

        public EventoController(IEventoService service)
        {
            this.service = service;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            try
            {
                var eventos = await service.GetEventoByIdAsync(id);
                if(eventos==null) return NotFound("Nenhum evento encontrado");
                return Ok(eventos);
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {

            try
            {
                var eventos = await service.GetAllEventosByTemaAsync(tema, true);
                if(eventos==null) return NotFound("Nenhum evento encontrado");
                return Ok(eventos);
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpGet]
        public  async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await service.GetAllEventosAsync();
                if(eventos==null) return NotFound("Nenhum evento encontrado");
                return Ok(eventos);
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBodyAttribute] Evento evento)
        {
            try
            {
                var eventos = await service.Add(evento);
                if(eventos==null) return BadRequest("Erro ao salvar evento");
                return Ok(eventos);
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                 var ok = await service.Delete(id);
                if(!ok) return BadRequest("Erro ao deletar evento");
                return Ok("Deletado");
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBodyAttribute] Evento evento)
        {
            try
            {
                var eventos = await service.Update(id, evento);
                if(eventos==null) return BadRequest("Erro ao atualizar evento");
                return Ok(eventos);
            }
            catch (Exception e)
            {
                
               return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }
    }
}
