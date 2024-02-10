using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Favoris.ApplicationCore.Interfaces;
using ModernRecrut.Favoris.ApplicationCore.Models;
using ModernRecrut.Favoris.ApplicationCore.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly IFavorisService _favorisService;
        public FavorisController(IFavorisService favorisService)
        {
            _favorisService = favorisService;
        }

        /// <summary>
        /// Retourne une liste de favoris 
        /// </summary>
        /// <param name="clee">clee d'identification du client</param>
        /// <response code="200">favoris trouvés et retournés</response>
        /// <response code="404">favoris introuvables</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET: api/<FavorisController>
        [HttpGet]
        [Route("{clee}")]
        public ActionResult Get(string clee)
        {
            var favoris = _favorisService.ObtenirTout(clee);
            if (favoris == null)
            {
                return NotFound();
            }

            return Ok(favoris.Contenu);
        }


        // GET api/<FavorisController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// Ajoute un favoris à la base de donnée
        /// </summary>
        /// <param name="transactionFavoris">un object possedant la clee et l'offre à ajouter</param>
        /// <response code="201">favoris ajouté avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">service indisponible pour le moment</response>
        // POST api/<FavorisController>
        [HttpPost]
        public ActionResult Post([FromBody] TransactionFavoris transactionFavoris)
        {
            if (ModelState.IsValid)
            {
                _favorisService.Ajouter(transactionFavoris);
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Supprime un favoris
        /// </summary>
        /// <param name="sup"> clee d'identification du client + id du favoris à supprimer. // format: clee,id</param>
        /// <response code="200">favoris supprimé avec succès</response>
        /// <response code="404">favoris introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // DELETE api/<FavorisController>/5
        [HttpDelete("{sup}")]
        
        public ActionResult Delete(string sup)
        {
            _favorisService.Supprimer(sup);
            return Ok();
        }
    }
}
