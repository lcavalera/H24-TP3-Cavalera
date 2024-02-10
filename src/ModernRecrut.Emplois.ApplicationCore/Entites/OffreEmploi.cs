using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Emplois.ApplicationCore.Entites
{
    public class OffreEmploi: BaseEntity
    {
        [Required(ErrorMessage = "Veuillez renseigner le nom de l'offre d'emploi")]
        public string Nom {  get; set; }
        [Required(ErrorMessage = "Veuillez renseigner le poste de l'offre d'emploi")]
        public string Poste { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner la description de l'offre d'emploi")]
        public string Description { get; set; }
        [DisplayName("Date d'affichage"), Required(ErrorMessage = "Veuillez renseigner la date d'affichage de l'offre d'emploi")]
        public DateTime DateAffichage { get; set; }
        [DisplayName("Date de Fin"), Required(ErrorMessage = "Veuillez renseigner la date de fin de l'offre d'emploi")]
        public DateTime DateFin {  get; set; }
    }
}
