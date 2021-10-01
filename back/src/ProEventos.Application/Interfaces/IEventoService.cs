using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDTO> Add(EventoDTO eventoDto);
        Task<EventoDTO> Update(int id, EventoDTO eventoDto);
        Task<bool> Delete(int eventoId);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}