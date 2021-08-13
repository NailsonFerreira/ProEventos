using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence

{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options){}
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        public DbSet<Palestrante> Palestrante { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>().HasKey(PE=>new {PE.EventoId, PE.PalestranteId});
        }
    }
}