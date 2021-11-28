using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly IProEventoBase baseRepository;
        private readonly ILoteRepository loteRepository;
        private readonly IMapper mapper;
        public LoteService(IProEventoBase baseRepository, ILoteRepository loteRepository, IMapper mapper)
        {
            this.loteRepository = loteRepository;
            this.baseRepository = baseRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Delete(int eventoId, int id)
        {
            try
            {
                var lote = await loteRepository.GetLoteByIdAsync(eventoId, id);
                if (lote == null) throw new Exception("Lote não encontrado");
                
                baseRepository.Delete<Lote>(lote);
                return await baseRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoteDTO> GetLoteByEventIdAsync(int eventoId, int id)
        {
            try
            {
                var lote = await loteRepository.GetLoteByIdAsync(eventoId, id);
                if (lote == null) return null;

                return mapper.Map<LoteDTO>(lote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoteDTO[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lote = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lote == null) return null;

                return mapper.Map<LoteDTO[]>(lote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoteDTO[]> Save(int eventoId, LoteDTO[] lotesDto)
        {
            try
            {
                var lotes = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in lotesDto)
                {
                    if (model.Id == 0)
                    {
                        await Add(eventoId, model);
                    }
                    else
                    {
                        await Update(eventoId, lotes, model);
                        // var lote = lotes.FirstOrDefault(x => x.Id == model.Id);
                        // model.EventoId = eventoId;
                        // mapper.Map(model, lote);

                        // baseRepository.Update<Lote>(lote);

                        // await baseRepository.SaveChangesAsync();
                    }
                }

                var lotesResponse = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                return mapper.Map<LoteDTO[]>(lotesResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task Update(int eventoId, Lote[] lotes, LoteDTO model)
        {
            try
            {
                var lote = lotes.FirstOrDefault(x => x.Id == model.Id);
                model.EventoId = eventoId;
                mapper.Map(model, lote);

                baseRepository.Update<Lote>(lote);

                await baseRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task Add(int eventoId, LoteDTO loteDto)
        {
            try
            {
                var lote = mapper.Map<Lote>(loteDto);
                lote.EventoId = eventoId;

                baseRepository.Add<Lote>(lote);

                await baseRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}