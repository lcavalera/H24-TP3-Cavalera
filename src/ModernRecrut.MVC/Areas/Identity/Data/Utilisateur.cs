using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ModernRecrut.MVC.Areas.Identity.Data;
public enum Type
{
    Employe,
    Candidat
}
// Add profile data for application users by adding properties to the Utilisateur class
public class Utilisateur : IdentityUser
{
    [Required]
    public string? Nom {  get; set; }
    [Required]
    [Display(Name ="Prénom")]
    public string? Prenom { get; set; }
    public Type Type { get; set; }
    [NotMapped]
    public string NomComplet { get { return Prenom + " " + Nom; }  }
}

