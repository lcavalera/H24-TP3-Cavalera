using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.Log;

namespace ModernRecrut.MVC.Services
{
    public class DocumentsServiceProxy : IDocumentsService
    {
        private readonly HttpClient _httpClient;
        private const string _documentsApiUrl = "http://localhost:7198/api/ajoutdocuments";
        private readonly List<Fichier> _fichiers;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DocumentsServiceProxy> _logger;

        public DocumentsServiceProxy(HttpClient httpClient, IConfiguration configuration, ILogger<DocumentsServiceProxy> logger)
        {
            _httpClient = httpClient;
            _fichiers = new List<Fichier>();
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Enregistrer(Fichier document, string filePath)
        {
            using (var stream = new MemoryStream())
            {
                await document.DocIFormFile.CopyToAsync(stream);
                document.DocumentConverti = Convert.ToBase64String(stream.ToArray());
                document.DocIFormFile = null;
            }

            var response = await _httpClient.PostAsJsonAsync(_documentsApiUrl, document);

            if (!response.IsSuccessStatusCode)
            {
                //Jurnalisation
                int statusCode = (int)response.StatusCode;
                if (statusCode >= 400 && statusCode < 500)
                {
                    //Jurnalisation comme erreur
                    _logger.LogError(DocumentsLog.Suppression, $"Erreur lors de l'ajout d'un document de type '{document.Type}'. Id Utilisateur = {document.IdUtilisateur}");
                }
                else if (statusCode >= 500 && statusCode < 600)
                {
                    //Jurnalisation comme erreur
                    _logger.LogCritical(DocumentsLog.Suppression, $"Erreur lors de l'ajout d'un document de type '{document.Type}'. Id Utilisateur = {document.IdUtilisateur}");
                }

                //Exception
                throw new HttpRequestException("Erreur lors de l'ajout d'un document");
            }
        }

        public async Task<List<Fichier>> ObtenirTout(string id)
        {
            var response = await _httpClient.GetAsync(_documentsApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                //Jurnalisation
                int statusCode = (int)response.StatusCode;
                if (statusCode >= 400 && statusCode < 500)
                {
                    //Jurnalisation comme erreur
                    _logger.LogError(DocumentsLog.Suppression, $"Erreur lors de la récupération des document de l'utilisateur. Id Utilisateur = {id}");
                }
                else if (statusCode >= 500 && statusCode < 600)
                {
                    //Jurnalisation comme erreur
                    _logger.LogCritical(DocumentsLog.Suppression, $"Erreur lors de la récupération des document de l'utilisateur. Id Utilisateur = {id}");
                }

                //Exception
                throw new HttpRequestException("Erreur lors de la récupération des documents");
            }
            List<string> fichiersApi = await response.Content.ReadFromJsonAsync<List<string>>();

            return CreationModel(fichiersApi);
        }

        public async Task<Fichier> ObtenirSelonId(int id, string idUtilisateur)
        {
            List<Fichier> fichiers = await ObtenirTout(idUtilisateur);

            return fichiers.SingleOrDefault(f => f.Id == id);
        }

        public async Task Supprimer(string nom)
        {
            var response = await _httpClient.DeleteAsync(_documentsApiUrl + nom);

            if (!response.IsSuccessStatusCode)
            {
                //Jurnalisation
                int statusCode =(int)response.StatusCode;
                if(statusCode >= 400 && statusCode < 500)
                {
                    //Jurnalisation comme erreur
                    _logger.LogError(DocumentsLog.Suppression, $"Erreur lors de la suppression d'un document. Nom = {nom}");
                }
                else if(statusCode >= 500 && statusCode < 600)
                {
                    //Jurnalisation comme erreur
                    _logger.LogCritical(DocumentsLog.Suppression, $"Erreur lors de la suppression d'un document. Nom = {nom}");
                }

                //Exception
                throw new HttpRequestException("Erreur lors de la suppression du document");
            }
        }
        private List<Fichier> CreationModel(List<string> fichiersApi)
        {
            int id = 1;

            foreach (string fichier in fichiersApi)
            {
                string link = _configuration.GetValue<string>("ConteneurDocuments") + fichier + "?" + _configuration.GetValue<string>("JetonAccess");

                string idUtilisateur = fichier.Substring(0, fichier.IndexOf("_"));

                string nom = fichier.Substring(fichier.IndexOf("_") + 1);
                string type = nom.Substring(0, nom.IndexOf("_"));

                FichierType fichierType = new FichierType();

                switch (type.ToLower())
                {
                    case "curriculum":
                        fichierType = FichierType.Curriculum;
                        break;

                    case "lettre":
                        fichierType = FichierType.Lettre;
                        break;

                    case "diplôme":
                        fichierType = FichierType.Diplôme;
                        break;

                    default:
                        fichierType = FichierType.Autre;
                        break;
                }

                Fichier fichierData = new Fichier
                {
                    Id = id,
                    IdUtilisateur = idUtilisateur,
                    Type = fichierType,
                    Nom = nom,
                    Link = link
                };
                _fichiers.Add(fichierData);
                id++;
            }
            return _fichiers;
        }
    }
}
