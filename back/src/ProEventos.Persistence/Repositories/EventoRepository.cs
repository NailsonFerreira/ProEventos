using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Persistence
{
    public class EventoRepository :IEventoRepository
    {
        private readonly ProEventosContext context;
        public EventoRepository(ProEventosContext context)
        {
            this.context = context;
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos.AsNoTracking()
                                        .Include(e => e.Lotes)
                                        .Include(e => e.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(pe => pe.Palestrante);
            }
            query = query.OrderBy(e => e.Id)
                        .Where(e => e.Id == eventoId);

            query = query.AsNoTracking().Where(x => x.UserId == userId).OrderBy(e => e.Id);


            return await query.FirstOrDefaultAsync();
        }    

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
                                              .Include(e => e.Lotes)
                                              .Include(e => e.RedesSociais)
                                              .OrderBy(e => e.Id);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().Where(x => x.UserId == userId).OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
                                              .Include(e => e.Lotes)
                                              .Include(e => e.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                            .ThenInclude(pe => pe.Palestrante);
            }
            query = query.OrderBy(e => e.Id)
                        .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            query = query.AsNoTracking().Where(x => x.UserId == userId).OrderBy(e => e.Id);


            return await query.ToArrayAsync();
        }
    }
}