using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Services
{
    public class OffresEmploiServiceProxy: IOffreEmploiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _offresEmploiApiUrl = "api/offreEmploi/";
        public OffresEmploiServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Ajouter(OffreEmploi offreEmploi)
        {
            var response = await _httpClient.PostAsJsonAsync(_offresEmploiApiUrl, offreEmploi);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de l'ajout d'une offre d'emploi");
            }
        }
        public async Task<OffreEmploi> ObtenirSelonId(int id)
        {
            var response = await _httpClient.GetAsync(_offresEmploiApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la récupération de l'offre d'emploi");
            }

            return await response.Content.ReadFromJsonAsync<OffreEmploi>();
        }
        public async Task<IEnumerable<OffreEmploi>> ObtenirTout()
        {
            // verifier si l'utilisateur a déposé au moi un cv et un lettre pour afficher la postulation

            var response = await _httpClient.GetAsync(_offresEmploiApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la récupération des offres d'emploi");
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<OffreEmploi>>();
        }
        public async Task Modifier(OffreEmploi offreEmploi)
        {
            var response = await _httpClient.PutAsJsonAsync(_offresEmploiApiUrl + offreEmploi.OffreEmploiID, offreEmploi);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la modification d'une offre d'emploi");
            }
        }
        public async Task Supprimer(OffreEmploi offreEmploi)
        {
            var response = await _httpClient.DeleteAsync(_offresEmploiApiUrl + offreEmploi.OffreEmploiID);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la suppression de l'offre d'emploi");
            }
        }
        public async Task<List<OffreEmploi>> OffresFiltrer(string filtre)
        {
            IEnumerable<OffreEmploi> offresEmploi = await ObtenirTout();

            if (!String.IsNullOrEmpty(filtre))
            {
                offresEmploi = offresEmploi.Where(o => o.Poste.ToLower().Contains(filtre.ToLower()));
            }

            return offresEmploi.ToList();
        }
    }
}
