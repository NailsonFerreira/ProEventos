using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Persistence
{
    public class PalestranteRecository :IPalestranteRepository
    {
        private readonly ProEventosContext context;
        public PalestranteRecository(ProEventosContext context)
        {
            this.context = context;

        }
        public async Task<Palestrante[]> GetAllPaleastrantesAsync(bool includeEventos = false)
        {
             IQueryable<Palestrante> query = context.Palestrante
                                              .Include(e => e.RedesSociais)
                                              .OrderBy(e => e.Id);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(e => e.Evento);
            }

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPaleastrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = context.Palestrante
                                            .Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(e => e.Evento);
            }
            query.OrderBy(e => e.Id).Where(e=>e.Nome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }
        
        public async Task<Palestrante> GetPaleastranteByIdAsync(int id, bool includeEventos = false)
        {
                        IQueryable<Palestrante> query = context.Palestrante
                                            .Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(e => e.Evento);
            }
            query.OrderBy(e => e.Id).Where(e=>e.Id==id);
            return await query.FirstOrDefaultAsync();
        }
    }
}