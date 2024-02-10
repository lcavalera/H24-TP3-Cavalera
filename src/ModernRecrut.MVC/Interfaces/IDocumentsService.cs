using Microsoft.VisualBasic.FileIO;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IDocumentsService
    {
        public Task Enregistrer(Fichier document, string filePath);
        public Task<List<Fichier>> ObtenirTout(string id);
        public Task<Fichier> ObtenirSelonId(int id, string idUtilisateur);
        public Task Supprimer(string filePath);
    }
}
