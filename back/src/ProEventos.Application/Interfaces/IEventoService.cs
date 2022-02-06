using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDTO> Add(int userId, EventoDTO eventoDto);
        Task<EventoDTO> Update(int userId, int id, EventoDTO eventoDto);
        Task<bool> Delete(int userId, int eventoId);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}