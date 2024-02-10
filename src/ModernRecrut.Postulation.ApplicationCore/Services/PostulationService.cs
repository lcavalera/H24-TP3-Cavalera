using ModernRecrut.Postulations.ApplicationCore.Entites;
using ModernRecrut.Postulations.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.ApplicationCore.Services
{
    public class PostulationService : IPostulationService
    {
        private readonly IAsyncRepository<Postulation> _postulationRepository;

        public PostulationService(IAsyncRepository<Postulation> postulationRepository)
        {
            _postulationRepository = postulationRepository;
        }
        public async Task Ajouter(Postulation item)
        {
            await _postulationRepository.AddAsync(item);
        }

        public async Task Modifier(Postulation item)
        {
            await _postulationRepository.EditAsync(item);
        }

        public async Task<Postulation> ObtenirSelonId(int id)
        {
            return await _postulationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Postulation>> ObtenirTout()
        {
            return await _postulationRepository.ListAsync();
        }

        public async Task Supprimer(Postulation item)
        {
            await _postulationRepository.DeleteAsync(item);
        }
    }
}
