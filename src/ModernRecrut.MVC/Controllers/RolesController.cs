using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models.DTO;
using ModernRecrut.MVC.Models.Log;
using System.Data;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRolesService _rolesService;
        public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
        {
            _rolesService = rolesService;
            _logger = logger;
        }
        // GET: RolesController
        public async Task<ActionResult<IEnumerable<IdentityRole>>> Index()
        {
            var roles = await _rolesService.ObtenirTout();
            if (roles != null)
            {
                _logger.LogInformation(RolesLog.Lecture, $"Retour de {roles.Count} roles");
                return View(roles);
                
            }
            _logger.LogError(RolesLog.Lecture, $"Erreur lors de la lecture des roles.");
            return BadRequest();



        }
        public async Task<IActionResult> Assigner()
        {
            ViewBag.Users = await _rolesService.RemplirSelectList_Users();
            ViewBag.Roles = await _rolesService.RemplirSelectList_Roles();
            return View();
        }
        // POST: RolesController/Assigner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assigner(UtilisateurRoleVueModel obj)
        {
            if (await _rolesService.RoleDejaAssigner(obj))
            {
                ModelState.AddModelError("RoleName", "Ce role est déjà assigné à cet utilisateur");
                _logger.LogError(RolesLog.Creation, $"Erreur lors de l'assignation du role {obj.RoleName} à l'utilisateur: UserID: {obj.UserId}");
            }
            if (ModelState.IsValid)
            {
                await _rolesService.Assigner(obj);
                _logger.LogInformation(RolesLog.Assignation, $"Role {obj.RoleName} assigné à UserID: {obj.UserId}");
                return RedirectToAction("Index");
            }
            ViewBag.Users = await _rolesService.RemplirSelectList_Users();
            ViewBag.Roles = await _rolesService.RemplirSelectList_Roles();
            return View(obj);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (await _rolesService.RoleExiste(role))
            {
                ModelState.AddModelError("Name", "Le role existe déjà");
                _logger.LogError(RolesLog.Creation, $"Erreur lors de l'ajout du role {role.Name} (créer par {User.Identity.Name})");
            }

            if (ModelState.IsValid)
            {
                await _rolesService.Ajouter(role);
                _logger.LogInformation(RolesLog.Creation, $"Role {role.Name} ajouté à la liste des roles (créer par {User.Identity.Name})");
                return RedirectToAction("Index");
            }

            return View(role);
        }
    }
}
