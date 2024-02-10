using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using ModernRecrut.MVC.Models.Log;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize]
    public class PostulationsController : Controller
    {
        private readonly IPostulationService _postulationService;
        private readonly ILogger<PostulationsController> _logger;
        public PostulationsController(ILogger<PostulationsController> logger, IPostulationService postulationService)
        {
            _logger = logger;
            _postulationService = postulationService;
        }
        [Authorize(Roles = "Employe, Admin")]
        // GET: Postulation
        public async Task<ActionResult> ListePostulations()
        {
            IEnumerable<ListePostulationVueModel> postulations = await _postulationService.ObtenirTout();

            if (postulations != null)
            {
                _logger.LogInformation(PostulationLog.Lecture, $"Lecture de {postulations.Count()} postulations");
                return View(postulations);
            }
            return Problem("postulations introuvables.");
        }
        [Authorize(Roles = "RH, Admin")]
        public async Task<ActionResult> Notes()
        {
            
            _logger.LogInformation(PostulationLog.Notation, "Notation d'une postulation");
            return View();
        }
        [Authorize(Roles = "Candidat, Admin")]
        // GET: Postulation/Create
        public async Task<ActionResult> Postuler(int Id)
        {
            Utilisateur utilisateur = await _postulationService.ObtenirUtilisateur(User.Identity.Name);
            Postulation postulation = new Postulation { IdCandidat= utilisateur.Id, OffreEmploiID = Id};
            postulation.IdCandidat = utilisateur.Id;
            return View(postulation);
        }
        [Authorize(Roles = "Candidat, Admin")]
        // POST: Postulation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Postuler([Bind("IdCandidat,OffreEmploiID,PretentionSalariale,DateDisponibilite,Notes")] Postulation postulation)
        {
            //ajouter validation date et prétention salariale

            await Validations_Postulation(postulation);
            Validations(postulation);
            if (ModelState.IsValid)
            {
                await _postulationService.Ajouter(postulation);
                _logger.LogInformation(PostulationLog.Postulation, "Postulation d'un candidat");
                return RedirectToAction(nameof(Index), controllerName: "OffreEmploi");
            }
            return View(postulation);
        }

        [Authorize(Roles = "RH, Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Postulation postulation = await _postulationService.ObtenirSelonId(id);

            if (postulation == null)
            {
                _logger.LogError($"Une erreur s'est produite lors de la récupération d'une postulation. ID = {id}");
                return NotFound();
            }
            return View(postulation);
        }

        //[Authorize(Roles = "RH, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,IdCandidat,OffreEmploiID,PretentionSalariale,DateDisponibilite,Notes")] Postulation postulation)
        {
            if (postulation == null)
            {
                _logger.LogError($"Une erreur s'est produite lors de la modification d'une postulation.");
                return NotFound();
            }

            ValidationDate_Modification_Supression(postulation.DateDisponibilite);
            Validations(postulation);
            if (ModelState.IsValid)
            {
                await _postulationService.Modifier(postulation);
                _logger.LogInformation(PostulationLog.Modification, $"Modification d'une postulation. ID = {postulation.Id}");
                return RedirectToAction(nameof(ListePostulations));
            }
            return View(postulation);
        }


        [Authorize(Roles = "RH, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Postulation postulation = await _postulationService.ObtenirSelonId(id);

            if (postulation == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une postulation. id recherché : {id}");
                return NotFound();
            }
            return View(postulation);
        }

        [Authorize(Roles = "RH, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Postulation postulation)
        {
            postulation = await _postulationService.ObtenirSelonId(postulation.Id);
            if (postulation == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une offre d'emploi.");
                return NotFound();
            }
            ValidationDate_Modification_Supression(postulation.DateDisponibilite);
            if (ModelState.IsValid)
            {
                await _postulationService.Supprimer(postulation.Id);
                _logger.LogInformation(PostulationLog.Suppression, $"Suppression d'une postulation. ID = {postulation.OffreEmploiID}");
                return RedirectToAction("ListePostulations");
            }
            return View(postulation);
        }





        //Ajouter methode Delete avec verification date
        private async Task Validations_Postulation(Postulation postulation)
        {
            if (!await _postulationService.CandidatPossedeLettre(postulation.IdCandidat))
            {
                ModelState.AddModelError("", "Vous devez déposer une lettre pour Postuler");
            }
            if (!await _postulationService.CandidatPossedeCV(postulation.IdCandidat))
            {
                ModelState.AddModelError("", "Vous devez déposer un CV pour Postuler");
            }
        }
        private void Validations(Postulation postulation)
        {
            
            if (_postulationService.PretentionsSalarialeTropElever(postulation.PretentionSalariale))
            {
                ModelState.AddModelError("PretentionSalariale", "Votre prétention salariale est au-delà de nos limites");
            }
            if (_postulationService.DateDisponibiliteIncorecte(postulation.DateDisponibilite))
            {
                ModelState.AddModelError("", $"La date de disponibilité doit être supérieure à {DateTime.Now:MM/dd/yyyy} et inférieure au {DateTime.Now.AddDays(45):MM/dd/yyyy}");
            }
        }
        private void ValidationDate_Modification_Supression(DateTime date)
        {
            if (_postulationService.DateIncorectePour_Modification_Supression(date))
            {
                ModelState.AddModelError("", "La postulation ne peut pas être modifiée car la date de l'entrevue est proche.");
            }
        }
    }
}
