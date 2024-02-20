using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Emplois.Infrastructure.Data
{
    public class GestOffreEmploiContext: DbContext
    {
        public GestOffreEmploiContext(DbContextOptions<GestOffreEmploiContext> options) : base(options)
        {

        }

        public DbSet<OffreEmploi>? OffresEmploi { get; set; }

    }
}
