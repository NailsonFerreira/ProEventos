using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Migrations
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}