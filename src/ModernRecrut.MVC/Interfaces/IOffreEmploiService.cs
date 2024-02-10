using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IOffreEmploiService
    {
        public Task Ajouter(OffreEmploi item);
        public Task<OffreEmploi> ObtenirSelonId(int id);
        public Task<IEnumerable<OffreEmploi>> ObtenirTout();
        public Task Modifier(OffreEmploi item);
        public Task Supprimer(OffreEmploi item);
        public Task<List<OffreEmploi>> OffresFiltrer(string filtre);
    }
}
