using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{

    public class UtilisateurRoleVueModel
    {
        [Display(Name ="Utilisateur")]
        public string UserId { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
