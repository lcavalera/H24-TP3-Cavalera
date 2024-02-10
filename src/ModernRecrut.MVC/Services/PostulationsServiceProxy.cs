using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace ModernRecrut.MVC.Services
{
    public class PostulationsServiceProxy : IPostulationService
    {
        private readonly IDocumentsService _documentsService;
        private readonly IOffreEmploiService _offresEmploiServiceProxy;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly HttpClient _httpClient;
        private const string _postulationsApiUrl = "api/postulation/";

        public PostulationsServiceProxy(HttpClient httpclient, UserManager<Utilisateur> userManager, IOffreEmploiService offresEmploiServiceProxy, IDocumentsService documentsService)
        {
            _httpClient = httpclient;
            _userManager = userManager;
            _offresEmploiServiceProxy = offresEmploiServiceProxy;
            _documentsService = documentsService;
        }
        public async Task<IEnumerable<ListePostulationVueModel>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_postulationsApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors des récupération d`une postulation");
            }

            return await TransitionVueModel(await response.Content.ReadFromJsonAsync<IEnumerable<Postulation>>());

        }
        public async Task<Postulation> ObtenirSelonId(int id)
        {
            var response = await _httpClient.GetAsync(_postulationsApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la récupération d`une postulation");
            }

            return await response.Content.ReadFromJsonAsync<Postulation>();
        }
        public async Task Ajouter(Postulation postulation)
        {
            var response = await _httpClient.PostAsJsonAsync(_postulationsApiUrl, postulation);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de l'ajout d'une postulation");
            }
        }
        public async Task Modifier(Postulation postulation)
        {
            var response = await _httpClient.PutAsJsonAsync(_postulationsApiUrl + postulation.Id, postulation);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la modification d'une postulation");
            }
        }
        public async Task Supprimer(int id)
        {
            var response = await _httpClient.DeleteAsync(_postulationsApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la suppression d'une postulation");
            }
        }
        public async Task<Utilisateur> ObtenirUtilisateur(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        private async Task<IEnumerable<ListePostulationVueModel>> TransitionVueModel(IEnumerable<Postulation> postulations)
        {
            List<ListePostulationVueModel> listePostulationsVue = new();
            foreach (Postulation postulation in postulations)
            {

                Utilisateur utilisateur = await _userManager.FindByIdAsync(postulation.IdCandidat);
                OffreEmploi offre = await _offresEmploiServiceProxy.ObtenirSelonId(postulation.OffreEmploiID);
                listePostulationsVue.Add(new ListePostulationVueModel()
                {
                    IdPostulation = postulation.Id,
                    NomCandidat = utilisateur.NomComplet,
                    DateDisponibilite = postulation.DateDisponibilite,
                    PretentionSalariale = postulation.PretentionSalariale,
                    NomDuPoste = offre.Nom
                });
            }
            return listePostulationsVue.AsEnumerable();
        }
        public async Task<bool> CandidatPossedeLettre(string IdUtilisateur)
        {
            bool CandidatPossedeDocuments = false;
           var docs = await _documentsService.ObtenirTout(IdUtilisateur);
            if (docs.Any(d => d.Type == FichierType.Lettre))
            {

                return !CandidatPossedeDocuments;
            }
            return CandidatPossedeDocuments;
        }
        public async Task<bool> CandidatPossedeCV(string IdUtilisateur)
        {
            bool CandidatPossedeDocuments = false;
            var docs = await _documentsService.ObtenirTout(IdUtilisateur);
            if (docs.Any(d => d.Type == FichierType.Curriculum))
            {
                return !CandidatPossedeDocuments;
            }
            return CandidatPossedeDocuments;
        }
        public bool PretentionsSalarialeTropElever(decimal pretentions)
        {
            return pretentions > 150000;
        }
        public bool DateDisponibiliteIncorecte(DateTime date)
        {
            return date > DateTime.Now && date <= DateTime.Now.AddDays(45) ? false : true;
        }
        public bool DateIncorectePour_Modification_Supression(DateTime date)
        {
            return date >= DateTime.Now.AddDays(-5).Date && date <= DateTime.Now.AddDays(5).Date ? true : false;
        }
    }
}
