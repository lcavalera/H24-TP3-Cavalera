using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{
    public class ListePostulationVueModel
    {
        public int IdPostulation {  get; set; }
        [DisplayName("Nom du Candidat")]
        public string NomCandidat { get; set; }
        [DisplayName("Nom du poste")]
        public string NomDuPoste { get; set; }
        [DisplayName("Prétention Salariale")]
        public decimal PretentionSalariale { get; set; }
        [DisplayName("Date de disponibilité pour une entrevue")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode =true)]
        public DateTime DateDisponibilite { get; set; }
    }
}
