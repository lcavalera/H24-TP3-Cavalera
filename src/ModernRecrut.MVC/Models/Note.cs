namespace ModernRecrut.MVC.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int PostulationId { get; set; }
        public string NotePostulation { get; set; }
        public string NomEmeteur { get; set; }
    }
}
