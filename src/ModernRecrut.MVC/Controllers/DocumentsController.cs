using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.Log;
using SQLitePCL;
using System.Security.Principal;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Candidat, Admin")]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService _documentsService;
        private readonly ILogger<DocumentsController> _logger;
        private readonly UserManager<Utilisateur> _userManager;

        public DocumentsController(IDocumentsService documentsService, ILogger<DocumentsController> logger, UserManager<Utilisateur> userManager)
        {
            _documentsService = documentsService;
            _logger = logger;
            _userManager = userManager;        }

        // GET: DocumentsController
        public async Task<IActionResult> Index()
        {

            string idUtilisateur = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

            IEnumerable<Fichier> fichiers = await _documentsService.ObtenirTout(idUtilisateur);

            if(fichiers != null)
            {
                _logger.LogInformation(DocumentsLog.Creation, $"Lecture de documents. Id Utilisateur = {idUtilisateur}");

                _logger.LogCritical($"L'application a rencontré un problème critique lors de la lecture des documents. Id Utilisateur = {idUtilisateur}");
                
                return View(fichiers);
            }

            _logger.LogError($"Une erreur c'est produite lors de la lecture des documents. Id Utilisateur = {idUtilisateur}");
            
            return Problem("Erreur lors de la recuperation des documents.");
        }

        // GET: DocumentsController/Create
        public async Task<IActionResult> Create()
        {
            CreerListe();
            return View();
        }

        // POST: DocumentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fichier document)
        {
            long size = document.DocIFormFile.Length;

            string filePath = "";

            document.Extension = Path.GetExtension(document.DocIFormFile.FileName);

            filePath = Path.GetTempFileName();

            document.IdUtilisateur = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

            if (document.Extension != ".pdf" && document.Extension != ".doc" && document.Extension != ".docx")
            {
                ModelState.AddModelError("DocIFormFile", "Le type de fichier n'est pas correct");
            }

            if (size <= 0)
            {
                ModelState.AddModelError("DocIFormFile", "La dimension du fichier n'est pas correct");
            }

            if (ModelState.IsValid)
            {

                await _documentsService.Enregistrer(document, filePath);

                _logger.LogInformation(DocumentsLog.Creation, $"Ajout d'un document. Nom = {document.NomComplet}");

                _logger.LogCritical($"L'application a rencontré un problème critique lors de l'ajout d'un document. Nom = {document.NomComplet}");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError($"Une erreur c'est produite lors de l'ajout d'un document. Nom = {document.NomComplet}");
            
            CreerListe();
            return View(document);
        }
        // GET: DocumentsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            string idUtilisateur = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

            Fichier fichier = await _documentsService.ObtenirSelonId(id, idUtilisateur);

            if (fichier == null)
            {
                //_logger.LogError($"Une erreur c'est produite lors de la récupération d'un document. ID = {fichier.Id}");
                return NotFound();
            }
            return View(fichier);
        }

        // POST: DocumentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Fichier document)
        {
            string idUtilisateur = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

            document = await _documentsService.ObtenirSelonId(id, idUtilisateur);
            if (document == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'un document. ID = {id}");
                return NotFound();
            }
            await _documentsService.Supprimer(document.NomComplet);

            _logger.LogInformation(DocumentsLog.Suppression, $"Suppression d'un document. Nom = {document.NomComplet}");
            _logger.LogCritical($"L'application a rencontré un problème critique lors de la suppression d'un document. Nom = {document.NomComplet}");

            return RedirectToAction(nameof(Index));
        }

        private void CreerListe()
        {
            ViewBag.Type = new SelectList(Enum.GetValues(typeof(FichierType)).Cast<FichierType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");
        }
    }
}
