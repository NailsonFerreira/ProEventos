using System.Threading.Tasks;
using ProEventos.Persistence.Migrations;

namespace ProEventos.Persistence
{
    public class ProEventoBase : IProEventoBase
    {
        private readonly ProEventosContext context;
        public ProEventoBase(ProEventosContext context)
        {
            this.context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entity) where T : class
        {
            context.RemoveRange(entity);
        }

        
        public async Task<bool> SaveChangesAsync() 
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }
    }
}