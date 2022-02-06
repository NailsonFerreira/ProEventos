using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService service;
        private readonly IAccountService accountService;

        public IWebHostEnvironment hostEnvironment { get; }

        public EventoController(IEventoService service, IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            this.service = service;
            this.hostEnvironment = hostEnvironment;
            this.accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            try
            {
                var userId = User.GetUserId();
                var eventos = await service.GetEventoByIdAsync(userId, id);
                if (eventos == null) return NoContent();
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
                var userId = User.GetUserId();
                var eventos = await service.GetAllEventosByTemaAsync(userId, tema, true);
                if (eventos == null) return NoContent();
                return Ok(eventos);
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.GetUserId();
                var eventos = await service.GetAllEventosAsync(userId);
                if (eventos == null) return NoContent();
                return Ok(eventos);
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBodyAttribute] EventoDTO eventoDto)
        {
            try
            {
                var userId = User.GetUserId();

                var evento = await service.Add(userId, eventoDto);
                if (evento == null) return BadRequest("Erro ao salvar evento");
                return Ok(evento);
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var userId = User.GetUserId();

                var evento = await service.GetEventoByIdAsync(userId, eventoId);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    DeleteImage(evento.ImagemUrl);
                    evento.ImagemUrl = await SaveImage(file);
                }
                var retunEvent = await service.Update(userId, eventoId, evento);

                return Ok(retunEvent);
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
                var userId = User.GetUserId();

                var evento = await service.GetEventoByIdAsync(userId, id);
                if (evento == null) return NoContent();


                if (await service.Delete(userId, id))
                {
                    DeleteImage(evento.ImagemUrl);
                    return Ok(new { message = "Deletado" });
                }

                throw new Exception("Erro ao deletar evento");
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBodyAttribute] EventoDTO evento)
        {
            try
            {
                var userId = User.GetUserId();

                var eventos = await service.Update(userId, id, evento);
                if (eventos == null) return BadRequest("Erro ao atualizar evento");
                return Ok(eventos);
            }
            catch (Exception e)
            {

                return BadRequest($"Ocorreu um erro. Error{e.Message} ");
            }
        }

        [NonAction]
        public void DeleteImage(string imagename)
        {
            var imagePath = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/images", imagename);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                                .Take(10)
                                                .ToArray()).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
