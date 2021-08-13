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

        public Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
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