using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModernRecrut.MVC.Models
{
    public class Postulation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public string IdCandidat { get; set; }
        public int OffreEmploiID { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal PretentionSalariale { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateDisponibilite { get; set; }
        public List<Note>? Notes { get; set; }
    }
}
