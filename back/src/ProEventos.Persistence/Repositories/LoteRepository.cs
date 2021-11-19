using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Persistence
{
    public class LoteRepository : ILoteRepository
    {
        private readonly ProEventosContext context;
        public LoteRepository(ProEventosContext context)
        {
            this.context = context;
        }

        public async Task<Lote> GetLoteByIdAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = context.Lotes;
            query = query.AsNoTracking()
            .Where(x=> x.EventoId == eventoId&& x.Id ==id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
             IQueryable<Lote> query = context.Lotes;
            query = query.AsNoTracking()
            .Where(x=> x.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}