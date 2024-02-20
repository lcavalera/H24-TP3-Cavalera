using ModernRecrut.Postulations.ApplicationCore.Entites;
using ModernRecrut.Postulations.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernRecrut.Postulations.ApplicationCore.Services
{
    public class GenererEvaluation: IGenererEvaluation
    {
        public Postulation AjouterNote(Postulation postulation)
        {
            string valeurNote;

            switch (postulation.PretentionSalariale)
            {
                case < 20000:
                    valeurNote = "Salaire inférieur à la norme";
                    break;
                case <= 39999:
                    valeurNote = "Salaire proche mais inférieur à la norme";
                    break; 
                case <= 79999:
                    valeurNote = "Salaire dans la norme";
                    break;
                case <= 99999:
                    valeurNote = "Salaire proche mais supérieur à la norme";
                    break;
                default:
                    valeurNote = "Salaire supérieur à la norme";
                    break;
            }
            Note note = new Note
            {
                NomEmeteur = "ApplicationPostulation",
                NotePostulation = valeurNote,

            };

            if (postulation.Notes == null)
            {
                postulation.Notes = new List<Note>();
            }

            postulation.Notes.Add(note);
            return postulation;
        }
    }
}
