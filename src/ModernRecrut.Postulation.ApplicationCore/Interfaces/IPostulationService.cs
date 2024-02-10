using ModernRecrut.Postulations.ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.ApplicationCore.Interfaces
{
    public interface IPostulationService
    {
        public Task Ajouter(Postulation item);
        public Task<Postulation> ObtenirSelonId(int id);
        public Task<IEnumerable<Postulation>> ObtenirTout();
        public Task Modifier(Postulation item);
        public Task Supprimer(Postulation item);
    }
}
