using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MerosWebApi.Core.Models;
using MerosWebApi.Persistence.Entites;

namespace MerosWebApi.Persistence
{
    public class MerosDbContext : DbContext
    {
        public DbSet<DatabaseUser> Users { get; set; }

        public MerosDbContext(DbContextOptions<MerosDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
