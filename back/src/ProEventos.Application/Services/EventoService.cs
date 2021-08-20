using System;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IProEventoBase baseRepository;
        private readonly IEventoRepository eventoRepository;
        public EventoService(IProEventoBase baseRepository, IEventoRepository eventoRepository)
        {
            this.eventoRepository = eventoRepository;
            this.baseRepository = baseRepository;

        }
        public async Task<Evento> Add(Evento evento)
        {
            try
            {
                baseRepository.Add<Evento>(evento);
                if (await baseRepository.SaveChangesAsync())
                {
                    return await eventoRepository.GetEventoByIdAsync(evento.Id);
                }
                return null;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<bool> Delete(int eventoId)
        {
            try
            {
                var entity = await eventoRepository.GetEventoByIdAsync(eventoId);
                if (entity == null) throw new Exception("Evento não encontrado");

                baseRepository.Delete<Evento>(entity);
                return await baseRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;
                return eventos;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes: includePalestrantes);
                if (eventos == null) return null;
                return eventos;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes: includePalestrantes);
                if (evento == null) return null;
                return evento;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<Evento> Update(int id, Evento evento)
        {
            try
            {
                var entity = await eventoRepository.GetEventoByIdAsync(id);
                if (entity == null) return null;
                evento.Id = id;
                baseRepository.Update<Evento>(evento);
                if (await baseRepository.SaveChangesAsync())
                {
                    return await eventoRepository.GetEventoByIdAsync(id);
                }
                return null;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}