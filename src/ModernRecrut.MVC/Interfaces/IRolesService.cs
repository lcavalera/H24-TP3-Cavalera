using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IRolesService
    {
        public Task<bool> RoleExiste(IdentityRole role);
        public Task<bool> RoleDejaAssigner(UtilisateurRoleVueModel obj);
        public Task<List<IdentityRole>> ObtenirTout();
        public Task<Utilisateur> ObtenirUtilisateurParId(string id);
        public Task Ajouter(IdentityRole role);
        public Task Assigner(UtilisateurRoleVueModel obj);
        public Task<SelectList> RemplirSelectList_Users();
        public Task<SelectList> RemplirSelectList_Roles();
    }
}
