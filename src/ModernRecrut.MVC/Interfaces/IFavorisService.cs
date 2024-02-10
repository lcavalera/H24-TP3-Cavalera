using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IFavorisService
    {
        public Task Ajouter(OffreEmploi offre, string clee);
        public Task<IEnumerable<OffreEmploi>> ObtenirTout(string clee);
        public Task Supprimer(int id, string clee);
        public Task<OffreEmploi> ObtenirSelonId(int id, string clee);
    }
}
