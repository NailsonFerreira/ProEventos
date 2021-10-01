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
        public async Task<EventoDTO> Add(EventoDTO eventoDto)
        {
            try
            {
                 var evento = mapper.Map<Evento>(eventoDto);
                baseRepository.Add<Evento>(evento);
                if (await baseRepository.SaveChangesAsync())
                {
                    var entity = await eventoRepository.GetEventoByIdAsync(evento.Id);
                    return mapper.Map<EventoDTO>(entity);
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
                if (entity == null) throw new Exception("EventoDTO não encontrado");
                
                baseRepository.Delete<Evento>(entity);
                return await baseRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;
               
                var eventoDTOs = mapper.Map<EventoDTO[]>(eventos);

                return eventoDTOs;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes: includePalestrantes);
                if (eventos == null) return null;

                 var eventoDTOs = mapper.Map<EventoDTO[]>(eventos);

                return eventoDTOs;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes: includePalestrantes);
                if (evento == null) return null;

                var eventoDto = mapper.Map<EventoDTO>(evento);


                return eventoDto;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDTO> Update(int id, EventoDTO eventoDto)
        {
            try
            {
                var entity = await eventoRepository.GetEventoByIdAsync(id);
                if (entity == null) return null;
                eventoDto.Id = id;
                mapper.Map(eventoDto, entity);
                baseRepository.Update<Evento>(entity);
                if (await baseRepository.SaveChangesAsync())
                {
                    var evento = await eventoRepository.GetEventoByIdAsync(id);
                    return mapper.Map<EventoDTO>(evento);;
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