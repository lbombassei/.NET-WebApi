﻿using System;
using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Context
{
    public class ProvaContext : DbContext, IProvaContext
    {
        public ProvaContext(DbContextOptions<ProvaContext> options)
            : base(options)
        { }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        
        public override DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }

}
