using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.Log;
using System;
using System.Net;
using System.Web;

namespace ModernRecrut.MVC.Controllers
{
    public class FavorisController : Controller
    {
        private readonly IFavorisService _favorisService;
        private readonly IOffreEmploiService _offreEmploiService;
        private readonly ILogger<FavorisController> _logger;
        public FavorisController(IFavorisService favorisService, IOffreEmploiService offreEmploiService, ILogger<FavorisController> logger)
        {
            _favorisService = favorisService;
            _offreEmploiService = offreEmploiService;
            _logger = logger;
        }

        // GET: FavorisController
        public async Task<IActionResult> Index()
        {
            string? ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                return BadRequest();
            }
            IEnumerable<OffreEmploi> offresEmploi = await _favorisService.ObtenirTout(ip);
            return offresEmploi != null ?
                        View(offresEmploi) :
                        Problem(" 'offresEmploi' est null.");
            
        }
        public async Task<ActionResult> Details(int id)
        {
            string? ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                return BadRequest();
            }
            OffreEmploi offre = await _favorisService.ObtenirSelonId(id, ip);
            if (offre == null)
            {
                return NotFound();
            }
            return View(offre);
        }

        // GET: FavorisController/Create
        public async Task<ActionResult> Create(int id)
        {
            
            OffreEmploi offre = await _offreEmploiService.ObtenirSelonId(id);
            if (offre == null)
            {
                return NotFound();
            }
            return View(offre);
        }

        //POST: FavorisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OffreEmploiID,Nom,Poste,Description,DateAffichage,DateFin")] OffreEmploi offre)
        {
            string? ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                return BadRequest();
            }
            IEnumerable<OffreEmploi> offresEmploi = await _favorisService.ObtenirTout(ip);
            if (offresEmploi.Any(o => o.OffreEmploiID == offre.OffreEmploiID))
            {
                ModelState.AddModelError("OffreEmploiID", "Cette offre est déjà dans vos favoris");
            }
            if (ModelState.IsValid)
            {

                await _favorisService.Ajouter(offre, ip);
                _logger.LogInformation(FavoriLog.Ajout, $"Ajout d'un favoris. ID = {offre.OffreEmploiID}");
                _logger.LogError($"Une erreur c'est produite lors de la création d'un favoris. ID = {offre.OffreEmploiID}");
                _logger.LogCritical($"L'application a rencontré un problème critique lors de la création d'un favoris. ID = {offre.OffreEmploiID}");

                return RedirectToAction(nameof(Index));
            }
            return View(offre);

        }
        //GET: FavorisController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string? ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                return BadRequest();
            }
            OffreEmploi offre = await _favorisService.ObtenirSelonId(id, ip);
;            if (offre == null)
            {
                return NotFound();
            }
            return View(offre);
        }

        //POST: FavorisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int offreEmploiID, OffreEmploi offre)
        {
            string? ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                return BadRequest();
            }
            await _favorisService.Supprimer(offreEmploiID, ip);
            _logger.LogInformation(FavoriLog.Suppression, $"Suppression d'un favoris. ID = {offreEmploiID}");
            _logger.LogError($"Une erreur c'est produite lors de la suppression d'un favoris. ID = {offreEmploiID}");
            _logger.LogCritical($"L'application a rencontré un problème critique lors de la suppression d'un favoris. ID = {offreEmploiID}");
            
            return RedirectToAction(nameof(Index));
        }
    }
}
