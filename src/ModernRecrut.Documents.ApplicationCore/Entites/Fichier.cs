using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Documents.ApplicationCore.Entites
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
