using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Application.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Context
{
    public interface IProvaContext
    {
        DbSet<Produtos> Produtos { get; set; }
        DbSet<Categorias> Categorias { get; set; }
        DbSet<Usuarios> Usuarios { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }

}
