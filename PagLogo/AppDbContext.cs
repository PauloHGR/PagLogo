﻿using Microsoft.EntityFrameworkCore;
using PagLogo.Models;

namespace PagLogo
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Generic>? Generics { get; set; }
        public virtual DbSet<Tradesman>? Tradesmans { get; set; }
    }
}