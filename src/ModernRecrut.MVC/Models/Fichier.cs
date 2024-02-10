using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ModernRecrut.MVC.Models
{
    public class Fichier
    {
        public int Id { get; set; }
        public string? IdUtilisateur { get; set; }

        [Required(ErrorMessage = "Veuillez choisir un document")]
        [JsonIgnore]
        public IFormFile DocIFormFile { get; set; }

        [DisplayName("Type de document"), Required(ErrorMessage = "Veuillez choisir un type de document")]
        public FichierType Type { get; set; }
        public string? Extension { get; set; }
        public string? DocumentConverti { get; set; }
        public string? Link { get; set; }
        public string? Nom { get; set; }
        public string NomComplet { get { return IdUtilisateur + "_" + Nom; } }

    }
}
