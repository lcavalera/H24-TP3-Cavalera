using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Emplois.ApplicationCore.Entites;
using ModernRecrut.Emplois.ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Emplois.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffreEmploiController : ControllerBase
    {
        private readonly IOffreEmploiService _offreEmplieService;

        public OffreEmploiController(IOffreEmploiService offreEmplieService)
        {
            _offreEmplieService = offreEmplieService;
        }

        /// <summary>
        /// Retourne une liste des offres emploi 
        /// </summary>
        /// <response code="200">offres emploi trouvés et retournés</response>
        /// <response code="404">offres emploi introuvables</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET: api/<OffreEmploiController>
        [HttpGet]
        public async Task<IEnumerable<OffreEmploi>> Get ()
        {
            return await _offreEmplieService.ObtenirTout();
        }

        /// <summary>
        /// Retourne une offre emploi spécifique à partir de son id
        /// </summary>
        /// <param name="id">id de l'offre emploi à retourner</param>
        /// <response code="200">offre emploi trouvé et retourné</response>
        /// <response code="404">offre emploi introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET api/<OffreEmploiController>/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            OffreEmploi offreEmploi = await _offreEmplieService.ObtenirSelonId(id);

            if (offreEmploi == null)
            {
                return NotFound();
            }

            return Ok(offreEmploi);
        }

        /// <summary>
        /// Ajoute une offre emploi à la base de donnée
        /// </summary>
        /// <param name="emprunt">l'offre emploi à ajouter</param>
        /// <response code="201">offre emploi ajouté avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">service indisponible pour le moment</response>
        // POST api/<OffreEmploiController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OffreEmploi offreEmploi)
        {
            if (ModelState.IsValid)
            {
                await _offreEmplieService.Ajouter(offreEmploi);

                return CreatedAtAction("Get", new { id = offreEmploi.Id }, offreEmploi);
            }

            return BadRequest();
        }

        /// <summary>
        /// Modification d'une offre emploi
        /// </summary>
        /// <param name="id">id de l'offre emploi à modifier</param>
        /// <param name="emprunt">l'offre emploi avec changement</param>
        /// <response code="200">offre emploi modifié avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="404">offre emploi introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // PUT api/<OffreEmploiController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] OffreEmploi offreEmploi)
        {
            if (id != offreEmploi.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _offreEmplieService.Modifier(offreEmploi);

                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Supprime une offre emploi
        /// </summary>
        /// <param name="id">id de l'offre emploi à supprimer</param>
        /// <response code="200">offre emploi supprimé avec succès</response>
        /// <response code="404">offre emploi introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // DELETE api/<OffreEmploiController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            OffreEmploi offreEmploi = await _offreEmplieService.ObtenirSelonId(id);

            if (offreEmploi == null)
            {
                return NotFound();
            }

            await _offreEmplieService.Supprimer(offreEmploi);

            return NoContent();
        }
    }
}
