using Microsoft.AspNetCore.Http;

namespace FunctionDocuments
{
    public class Fichier
    {
        public int? Id { get; set; }
        public string? IdUtilisateur { get; set; }
        public IFormFile? DocIFormFile { get; set; }
        public FichierType Type { get; set; }
        public string? Extension { get; set; }
        public string? DocumentConverti { get; set; }
        public string? Link { get; set; }
        public string? Nom { get; set; }
    }
}