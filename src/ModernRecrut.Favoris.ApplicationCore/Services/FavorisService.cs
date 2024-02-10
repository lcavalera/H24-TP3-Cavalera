using ModernRecrut.Favoris.ApplicationCore.Interfaces;
using ModernRecrut.Favoris.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using ModernRecrut.Favoris.ApplicationCore.Models.DTO;

namespace ModernRecrut.Favoris.ApplicationCore.Services
{
    public class FavorisService : IFavorisService
    {
        private readonly IMemoryCache _memoryCache;
        public FavorisService(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }
        public void Ajouter(TransactionFavoris tf) // async ?
        {
            Models.Favoris favoris = ObtenirTout(tf.Clee);
            favoris.Contenu.Add(tf.Offre);
            Sauvegarde(favoris, tf.Clee);
        }

        public Models.Favoris ObtenirTout(string clee) // async ?
        {
            if(!_memoryCache.TryGetValue(clee, out Models.Favoris favoris))
            {
                favoris = new Models.Favoris()
                {
                    Id = clee,
                    Contenu = new List<OffreEmploi>()
                };
            }
            return favoris;
        }

        public void Supprimer(string sup)
        {
            string clee = sup.Split(',')[0];
            int id = int.Parse(sup.Split(',')[1]);
            Models.Favoris favoris = ObtenirTout(clee);
            OffreEmploi offre = favoris.Contenu.First(o => o.Id == id); // control
            favoris.Contenu.Remove(offre);
            Sauvegarde(favoris, clee);
        }
        private void Sauvegarde(Models.Favoris favoris, string clee)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(6),
                    Size = CalculerCharacteres(favoris),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                };
                _memoryCache.Set(clee, favoris, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                throw new InsufficientMemoryException($"{ex.Message}");
            }
        }
        private static int CalculerCharacteres(Models.Favoris favoris)
        {
            int count = 0;
            foreach (OffreEmploi offre in favoris.Contenu)
            {
                count += offre.Id.ToString().Length;
                count += offre.Poste.ToString().Length;
                count += offre.Nom.ToString().Length;
                count += offre.Description.ToString().Length;
                count += offre.DateAffichage.ToString().Length;
                count += offre.DateFin.ToString().Length;
            }
            count += favoris.Id.ToString().Length;
            return count;
        }
    }
}
