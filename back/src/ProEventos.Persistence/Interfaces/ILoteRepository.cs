using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Migrations
{
    public interface ILoteRepository
    {
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote> GetLoteByIdAsync(int eventoId, int id);
    }
}