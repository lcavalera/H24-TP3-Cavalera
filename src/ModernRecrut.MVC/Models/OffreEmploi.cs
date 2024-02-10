using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ModernRecrut.MVC.Models
{
    public class OffreEmploi
    {
        [JsonPropertyName("id")]
        public int OffreEmploiID { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner le nom de l'offre d'emploi")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner le poste de l'offre d'emploi")]
        public string Poste { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner la description de l'offre d'emploi")]
        public string Description { get; set; }
        [DisplayName("Date d'affichage"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Required(ErrorMessage = "Veuillez renseigner la date d'affichage de l'offre d'emploi")]
        public DateTime DateAffichage { get; set; }
        [DisplayName("Date de Fin"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Required(ErrorMessage = "Veuillez renseigner la date de fin de l'offre d'emploi")]
        public DateTime DateFin { get; set; }
    }
}
