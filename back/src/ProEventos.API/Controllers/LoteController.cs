using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoteController : ControllerBase
    {
        private readonly ILoteService service;

        public LoteController(ILoteService service)
        {
            this.service = service;

        }

        [HttpGet("eventoId")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await service.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();
                return Ok(lotes);
            }
            catch (Exception e)
            {
                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lotes = await service.GetLoteByEventIdAsync(eventoId, loteId);
                if (lotes == null) return NoContent();

                var ok = await service.Delete(eventoId, loteId);
                if (!ok) return BadRequest("Erro ao deletar lote");
                return Ok(new { message = "Deletado" });
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpPut("eventoId")]
        public async Task<IActionResult> Save(int eventoId, [FromBodyAttribute] LoteDTO[] models)
        {
            try
            {
                var lotes = await service.Save(eventoId, models);
                if (lotes == null) return BadRequest("Erro ao atualizar lotes");
                return Ok(lotes);
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }
    }
}
