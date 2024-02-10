using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.ApplicationCore.Entites
{
    public class Note: BaseEntity
    {
        public int PostulationId { get; set; }
        public string NotePostulation { get; set; }
        public string NomEmeteur { get; set; }

    }
}
