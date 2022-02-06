using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class UserRepository : ProEventoBase, IUserRepository
    {

        private readonly ProEventosContext context;
        public UserRepository(ProEventosContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await context.Users.Where(x => x.UserName.ToLower() == username.ToLower()).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await context.Users.ToArrayAsync();
        }

    }
}
