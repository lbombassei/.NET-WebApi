using Microsoft.EntityFrameworkCore;

namespace Application.Entities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Categorias> Categoria { get; set; }
        public DbSet<Produtos> Produto { get; set; }
        // Adicione outras entidades que você precise aqui

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defina configurações para suas entidades aqui, como chaves primárias, índices, etc.
        }
    }
}