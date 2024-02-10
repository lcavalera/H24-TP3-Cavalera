using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models.DTO;
using System.Data;

namespace ModernRecrut.MVC.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Utilisateur> _userManager;
        public RolesService(RoleManager<IdentityRole> roleManager, UserManager<Utilisateur> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<List<IdentityRole>> ObtenirTout()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public async Task Ajouter(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }
        public async Task<Utilisateur> ObtenirUtilisateurParId(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task Assigner(UtilisateurRoleVueModel obj)
        {
            Utilisateur user = await ObtenirUtilisateurParId(obj.UserId);

            await _userManager.AddToRoleAsync(user, obj.RoleName);
        }
        public async Task<bool> RoleExiste(IdentityRole identityRole)
        {
            return await _roleManager.Roles.AnyAsync(r => r.Name.ToLower() == identityRole.Name.ToLower());
        }
        public async Task<bool> RoleDejaAssigner(UtilisateurRoleVueModel obj)
        {
            Utilisateur user = await ObtenirUtilisateurParId(obj.UserId);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Any(r => r.ToLower().Equals(obj.RoleName.ToLower()));
        }
        public async Task<SelectList> RemplirSelectList_Users()
        {
            return new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
        }
        public async Task<SelectList> RemplirSelectList_Roles()
        {
            return new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
        }
    }
}
