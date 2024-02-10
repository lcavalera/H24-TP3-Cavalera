using ModernRecrut.Emplois.ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Emplois.ApplicationCore.Interfaces
{
    public interface IOffreEmploiService
    {
        public Task Ajouter(OffreEmploi item);
        public Task<OffreEmploi> ObtenirSelonId(int id);
        public Task<IEnumerable<OffreEmploi>> ObtenirTout();
        public Task Modifier(OffreEmploi item);
        public Task Supprimer(OffreEmploi item);
    }
}
