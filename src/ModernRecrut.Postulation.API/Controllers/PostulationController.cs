using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModernRecrut.Postulations.ApplicationCore.Entites;
using ModernRecrut.Postulations.ApplicationCore.Interfaces;
using ModernRecrut.Postulations.ApplicationCore.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Postulations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulationController : ControllerBase
    {
        private readonly IPostulationService _postulationService;
        private readonly IGenererEvaluation _genererEvaluation;

        public PostulationController(IPostulationService postulationService, IGenererEvaluation genererEvaluation)
        {
            _postulationService = postulationService;
            _genererEvaluation = genererEvaluation;
        }

        /// <summary>
        /// Retourne une liste des postulations 
        /// </summary>
        /// <response code="200">postulation trouvés et retournés</response>
        /// <response code="404">postulations introuvables</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET: api/<PostulationController>
        [HttpGet]
        public async Task<IEnumerable<Postulation>> Get()
        {
            return await _postulationService.ObtenirTout();
        }

        /// <summary>
        /// Retourne une postulation spécifique à partir de son id
        /// </summary>
        /// <param name="id">id de la postulation à retourner</param>
        /// <response code="200">postulation trouvé et retourné</response>
        /// <response code="404">postulation introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET api/<PostulationController>/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Postulation postulation = await _postulationService.ObtenirSelonId(id);

            if (postulation == null)
            {
                return NotFound();
            }

            return Ok(postulation);
        }

        /// <summary>
        /// Ajoute une postulation à la base de donnée
        /// </summary>
        /// <param name="postulation">la postulation à ajouter</param>
        /// <response code="201">postulation ajouté avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">service indisponible pour le moment</response>
        // POST api/<PostulationController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Postulation postulation)
        {
            if(postulation.DateDisponibilite <= DateTime.Now && postulation.DateDisponibilite > DateTime.Now.AddDays(45))
            {
                ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure au {DateTime.Now.AddDays(45)}.");
            }

            if(postulation.PretentionSalariale > 150000)
            {
                ModelState.AddModelError("PretentionSalariale", "Votre prétention salariale est au-delà de nos limites.");
            }

            if (ModelState.IsValid)
            {
                Postulation postulationMod = _genererEvaluation.AjouterNote(postulation);
                await _postulationService.Ajouter(postulationMod);
                return CreatedAtAction("Get", new { id = postulation.Id }, postulation);
            }

            return BadRequest();
        }

        /// <summary>
        /// Modification d'une postulation
        /// </summary>
        /// <param name="id">id de la postulation à modifier</param>
        /// <param name="postulation">la postulation avec changement</param>
        /// <response code="200">postulation modifié avec succès</response>
        /// <response code="204">postulation modifié avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="404">postulation introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // PUT api/<PostulationController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Postulation postulation)
        {
            if (id != postulation.Id)
            {
                return BadRequest();
            }

            if (postulation.DateDisponibilite >= DateTime.Now.AddDays(-5) && postulation.DateDisponibilite <= DateTime.Now.AddDays(5))
            {
                ModelState.AddModelError("DateDisponibilite", "La postulation ne peut pas être modifiée car la date de l'entrevue est proche");
            }

            if (postulation.DateDisponibilite <= DateTime.Now && postulation.DateDisponibilite > DateTime.Now.AddDays(45))
            {
                ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure au {DateTime.Now.AddDays(45)}.");
            }

            if (postulation.PretentionSalariale > 150000)
            {
                ModelState.AddModelError("PretentionSalariale", "Votre prétention salariale est au-delà de nos limites.");
            }

            if (ModelState.IsValid)
            {
                await _postulationService.Modifier(postulation);

                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Supprime une postulation
        /// </summary>
        /// <param name="id">id de la postulation à supprimer</param>
        /// <response code="200">postulation supprimé avec succès</response>
        /// <response code="204">postulation supprimé avec succès</response>
        /// <response code="404">postulation introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // DELETE api/<PostulationController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Postulation postulation = await _postulationService.ObtenirSelonId(id);

            if (postulation == null)
            {
                return NotFound();
            }

            if (postulation.DateDisponibilite >= DateTime.Now.AddDays(-5) && postulation.DateDisponibilite <= DateTime.Now.AddDays(5))
            {
                ModelState.AddModelError("DateDisponibilite", "La postulation ne peut pas être modifiée car la date de l'entrevue est proche");
            }

            if (ModelState.IsValid)
            {
                await _postulationService.Supprimer(postulation);

                return NoContent();
            }

            return BadRequest();
        }
    }
}
