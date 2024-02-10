using ModernRecrut.Emplois.ApplicationCore.Entites;
using ModernRecrut.Emplois.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Emplois.ApplicationCore.Services
{
    public class OffreEmploiService : IOffreEmploiService
    {
        private readonly IAsyncRepository<OffreEmploi> _offresEmploiRepository;

        public OffreEmploiService(IAsyncRepository<OffreEmploi> offresEmploiRepository)
        {
            _offresEmploiRepository = offresEmploiRepository;
        }

        public async Task Ajouter(OffreEmploi item)
        {
            await _offresEmploiRepository.AddAsync(item);
        }

        public async Task<OffreEmploi> ObtenirSelonId(int id)
        {
            return await _offresEmploiRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OffreEmploi>> ObtenirTout()
        {
            return await _offresEmploiRepository.ListAsync(o => o.DateAffichage <= DateTime.Now && o.DateFin >= DateTime.Now);
        }
        public async Task Modifier(OffreEmploi item)
        {
            await _offresEmploiRepository.EditAsync(item);
        }
        public async Task Supprimer(OffreEmploi item)
        {
            await _offresEmploiRepository.DeleteAsync(item);
        }
    }
}
