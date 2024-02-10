using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace ModernRecrut.MVC.Services
{
    public class FavorisServiceProxy: IFavorisService
    {
        private readonly HttpClient _httpClient;
        private const string _favorisApiUrl = "api/Favoris/";

        public FavorisServiceProxy(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }
        public async Task<IEnumerable<OffreEmploi>> ObtenirTout(string clee)
        {
            var response = await _httpClient.GetAsync(_favorisApiUrl+ clee);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la récupération des favoris");
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<OffreEmploi>>();
        }
        public async Task<OffreEmploi> ObtenirSelonId(int id, string clee)
        {
            IEnumerable<OffreEmploi> offres = await ObtenirTout(clee);
            
            return offres.Single(o => o.OffreEmploiID == id);
        }
        public async Task Ajouter(OffreEmploi offre, string clee)
        {

            TransactionFavoris tf = new()
            {
                Clee = clee,
                Offre = offre
            };
            var response = await _httpClient.PostAsJsonAsync(_favorisApiUrl, tf);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de l'ajout d'un favoris");
            }
        }
        public async Task Supprimer(int id, string clee)
        {
            string sup = clee + "," + id.ToString();
            var response = await _httpClient.DeleteAsync(_favorisApiUrl + sup);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erreur lors de la suppression du favoris");
            }
        }
    }
}
