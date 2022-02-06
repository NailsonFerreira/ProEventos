using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IProEventoBase baseRepository;
        private readonly IEventoRepository eventoRepository;
        private readonly IMapper mapper;
        public EventoService(IProEventoBase baseRepository, IEventoRepository eventoRepository, IMapper mapper)
        {
            this.eventoRepository = eventoRepository;
            this.baseRepository = baseRepository;
            this.mapper = mapper;
        }
        public async Task<EventoDTO> Add(int userId, EventoDTO eventoDto)
        {
            try
            {
                 var evento = mapper.Map<Evento>(eventoDto);
                evento.UserId = userId;

                baseRepository.Add<Evento>(evento);
                if (await baseRepository.SaveChangesAsync())
                {
                    var entity = await eventoRepository.GetEventoByIdAsync(userId, evento.Id);
                    return mapper.Map<EventoDTO>(entity);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            
            }
        }

        public async Task<bool> Delete(int userId, int eventoId)
        {
            try
            {
                var evento = await eventoRepository.GetEventoByIdAsync(userId, eventoId);
                if (evento == null) throw new Exception("EventoDTO não encontrado");

                evento.UserId = userId;
                evento.Id = eventoId;

                baseRepository.Delete<Evento>(evento);
                return await baseRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosAsync(userId, includePalestrantes);
                if (eventos == null) return null;
               
                var eventoDTOs = mapper.Map<EventoDTO[]>(eventos);

                return eventoDTOs;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosByTemaAsync(userId, tema, includePalestrantes: includePalestrantes);
                if (eventos == null) return null;

                 var eventoDTOs = mapper.Map<EventoDTO[]>(eventos);

                return eventoDTOs;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoRepository.GetEventoByIdAsync(userId, eventoId, includePalestrantes: includePalestrantes);
                if (evento == null) return null;

                var eventoDto = mapper.Map<EventoDTO>(evento);


                return eventoDto;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO> Update(int userId, int id, EventoDTO eventoDto)
        {
            try
            {
                var evento = await eventoRepository.GetEventoByIdAsync(userId, id);
                if (evento == null) return null;
                eventoDto.Id = id;
                eventoDto.UserId = userId;

                mapper.Map(eventoDto, evento);
                baseRepository.Update<Evento>(evento);
                if (await baseRepository.SaveChangesAsync())
                {
                    var eventoReturn = await eventoRepository.GetEventoByIdAsync(userId, id);
                    return mapper.Map<EventoDTO>(eventoReturn);;
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