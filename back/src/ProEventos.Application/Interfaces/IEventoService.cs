using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<Evento> Add(Evento evento);
        Task<Evento> Update(int id, Evento evento);
        Task<bool> Delete(int eventoId);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}