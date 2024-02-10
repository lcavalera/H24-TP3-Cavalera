using ModernRecrut.Documents.ApplicationCore.Entites;

namespace ModernRecrut.Documents.ApplicationCore.Interfaces
{
    public interface IStorageServiceHelper
    {
        public Task EnregistrerDocument(Fichier document);

        public Task<List<string>> ObtenirDocumentsDansConteneur(string idUtilisateur);

        public Task Supprimer(string nomDocument);

    }
}
