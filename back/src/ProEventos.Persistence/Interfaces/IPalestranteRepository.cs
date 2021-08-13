using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Migrations
{
    public interface IPalestranteRepository
    {
        Task<Palestrante[]> GetAllPaleastrantesByNomeAsync(string nome, bool includeEventos);
        Task<Palestrante[]> GetAllPaleastrantesAsync( bool includeEventos);
        Task<Palestrante> GetPaleastranteByIdAsync(int idPalestrante, bool includeEventos);

    }
}