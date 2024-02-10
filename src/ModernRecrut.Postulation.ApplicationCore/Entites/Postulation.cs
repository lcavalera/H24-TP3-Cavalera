using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.ApplicationCore.Entites
{
    public class Postulation : BaseEntity
    { 
        public string IdCandidat { get; set; } 
        public int OffreEmploiID { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal PretentionSalariale { get; set; }
        public DateTime DateDisponibilite { get; set; }
        public virtual List<Note>? Notes { get; set; } = new List<Note>();
    }
}
