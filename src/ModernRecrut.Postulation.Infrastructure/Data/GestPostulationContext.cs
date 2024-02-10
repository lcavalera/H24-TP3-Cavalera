using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.Infrastructure.Data
{
    public class GestPostulationContext: DbContext
    {
        public GestPostulationContext(DbContextOptions<GestPostulationContext> options) : base(options)
        {

        }

        public DbSet<Postulation> Postulation { get; set; }
        public DbSet<Note> Note { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Postulation>().Property(p => p.PretentionSalariale).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
