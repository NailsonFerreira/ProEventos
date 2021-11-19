using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<LoteDTO[]> Save(int eventoId, LoteDTO[] lotesDto);
        Task<bool> Delete(int eventoId, int id);
        Task<LoteDTO[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDTO> GetLoteByEventIdAsync(int eventoId, int id);
    }
}