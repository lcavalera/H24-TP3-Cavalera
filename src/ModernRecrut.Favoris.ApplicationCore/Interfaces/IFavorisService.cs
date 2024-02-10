using ModernRecrut.Favoris.ApplicationCore.Models;
using ModernRecrut.Favoris.ApplicationCore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Favoris.ApplicationCore.Interfaces
{
    public interface IFavorisService
    {
        public void Ajouter(TransactionFavoris tf);
        public Models.Favoris ObtenirTout(string clee);
        public void Supprimer(string sup);
    }
}
