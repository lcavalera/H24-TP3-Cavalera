using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IPostulationService
    {
        public Task Ajouter(Postulation postulation);
        public Task<IEnumerable<ListePostulationVueModel>> ObtenirTout();
        public Task Supprimer(int id);
        public Task Modifier(Postulation postulation);
        public Task<Postulation> ObtenirSelonId(int id);
        public Task<Utilisateur> ObtenirUtilisateur(string name);
        public Task<bool> CandidatPossedeLettre(string IdUtilisateur);
        public Task<bool> CandidatPossedeCV(string IdUtilisateur);
        public bool PretentionsSalarialeTropElever(decimal pretentions);
        public bool DateDisponibiliteIncorecte(DateTime date);
        public bool DateIncorectePour_Modification_Supression(DateTime date);
    }
}
