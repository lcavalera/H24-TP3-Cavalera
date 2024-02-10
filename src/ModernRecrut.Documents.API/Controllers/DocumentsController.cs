using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Documents.ApplicationCore.Entites;
using ModernRecrut.Documents.ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Documents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IStorageServiceHelper _storageServiceHelper;

        public DocumentsController(IStorageServiceHelper storageServiceHelper)
        {
            _storageServiceHelper = storageServiceHelper;
        }


        /// <summary>
        /// Retourne une liste des documents selon Id Utilisateur 
        /// </summary>
        ///  <param name="idUtilisateur">Id de l'utilisateur pour le quel on veut recuperer les documents</param>
        /// <response code="200">documents trouvés et retournés</response>
        /// <response code="404">documents introuvables</response>
        /// <response code="500">service indisponible pour le moment</response>
        // GET api/<DocumentsController>/5
        [HttpGet]
        [Route("{idUtilisateur}")]
        public async Task<List<string>> Get(string idUtilisateur)
        {
            return await _storageServiceHelper.ObtenirDocumentsDansConteneur(idUtilisateur);
        }

        /// <summary>
        /// Ajoute une document dans une répertoire local
        /// </summary>
        /// <param name="fileData">document a ajouter</param>
        /// <response code="201">document ajouté avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">service indisponible pour le moment</response>
        // POST api/<DocumentsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Fichier fileData)
        {
            if (ModelState.IsValid)
            {
                await _storageServiceHelper.EnregistrerDocument(fileData);

                return Ok();
            }

            return BadRequest();
        }
        /// <summary>
        /// Supprime un document
        /// </summary>
        /// <param name="nomDocument">nom du document à supprimer</param>
        /// <response code="200">document supprimé avec succès</response>
        /// <response code="404">document introuvable pour le nom spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        // DELETE api/<DocumentsController>/5
        [HttpDelete("{nomDocument}")]
        public async Task<ActionResult> Delete(string nomDocument)
        {
            if (nomDocument == null)
            {
                return NotFound();
            }

            await _storageServiceHelper.Supprimer(nomDocument);

            return Ok();
        }
    }
}
