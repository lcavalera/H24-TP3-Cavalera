using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.Log;
using System.Data;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "RH, Admin")]
    public class OffreEmploiController : Controller
    {

        private readonly IOffreEmploiService _offreEmploiService;
        private readonly ILogger<OffreEmploiController> _logger;

        public OffreEmploiController(IOffreEmploiService offreEmploiService, ILogger<OffreEmploiController> logger)
        {
            _offreEmploiService = offreEmploiService;
            _logger = logger;
        }

        [AllowAnonymous]
        // GET: OffreEmploiController
        public async Task<IActionResult> Index(string filtre)
        {
            if (filtre is null)
            {
                return View(await _offreEmploiService.ObtenirTout());
            }
            ViewData["actifFiltre"] = filtre;

            List<OffreEmploi> offresEmploi = await _offreEmploiService.OffresFiltrer(filtre);

            return offresEmploi != null ?
                        View(offresEmploi) :
                        Problem("Entité set 'offresEmploi' est null.");
        }

        [AllowAnonymous]
        // GET: OffreEmploiController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            OffreEmploi offreEmploi = await _offreEmploiService.ObtenirSelonId(id);

            if (offreEmploi == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
                return NotFound();

            }
            return View(offreEmploi);
        }

        // GET: OffreEmploiController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: OffreEmploiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OffreEmploiID,Nom,Poste,Description,DateAffichage,DateFin")] OffreEmploi offreEmploi)
        {
            if (ModelState.IsValid)
            {
                await _offreEmploiService.Ajouter(offreEmploi);
                _logger.LogInformation(OffreEmploiLog.Creation, $"Creation d'une offre. ID = {offreEmploi.OffreEmploiID}");
                
                _logger.LogCritical($"L'application a rencontré un problème critique lors de la création d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}"); 
                
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"Une erreur c'est produite lors de la création d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
            return View(offreEmploi);
        }

        // GET: OffreEmploiController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            OffreEmploi offreEmploi = await _offreEmploiService.ObtenirSelonId(id);

            if (offreEmploi == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
                return NotFound();
            }
            return View(offreEmploi);
        }

        // POST: OffreEmploiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OffreEmploiID,Nom,Poste,Description,DateAffichage,DateFin")] OffreEmploi offreEmploi)
        {
            if (offreEmploi == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la modification d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _offreEmploiService.Modifier(offreEmploi);
                _logger.LogInformation(OffreEmploiLog.Modication, $"Modification d'une offre. ID = {offreEmploi.OffreEmploiID}");
                
                _logger.LogCritical($"L'application a rencontré un problème critique lors de la modification d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}"); 
                
                return RedirectToAction(nameof(Index));
            }

            return View(offreEmploi);
        }

        // GET: OffreEmploiController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            OffreEmploi offreEmploi = await _offreEmploiService.ObtenirSelonId(id);

            if (offreEmploi == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
                return NotFound();
            }
            return View(offreEmploi);
        }

        // POST: OffreEmploiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, OffreEmploi offreEmploi)
        {
            offreEmploi = await _offreEmploiService.ObtenirSelonId(id);
            if (offreEmploi == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}");
                return NotFound();
            }
            await _offreEmploiService.Supprimer(offreEmploi);
            _logger.LogInformation(OffreEmploiLog.Suppression, $"Suppression d'une offre. ID = {offreEmploi.OffreEmploiID}");
            _logger.LogCritical($"L'application a rencontré un problème critique lors de la suppression d'une offre d'emploi. ID = {offreEmploi.OffreEmploiID}"); 
            
            return RedirectToAction(nameof(Index));
        }
    }
}
