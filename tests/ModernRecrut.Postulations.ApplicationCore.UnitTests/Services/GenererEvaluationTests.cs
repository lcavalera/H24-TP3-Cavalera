using AutoFixture;
using FluentAssertions;
using ModernRecrut.Postulations.ApplicationCore.Entites;
using ModernRecrut.Postulations.ApplicationCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ModernRecrut.Postulations.ApplicationCore.UnitTests.Services
{
    public class GenererEvaluationTests
    {
        [Fact]
        public void AjouterNote_PretentionSalarialeInferieurA20000_GenereNoteSalaireInferieureNorme()
        {
            //Etant donné

            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.PretentionSalariale = 15000;

            //Initialiser la classe à tester
            GenererEvaluation genererEvaluation = new GenererEvaluation();

            //Lorsque
            var postulationAvecNote = genererEvaluation.AjouterNote(postulation);
            var note = postulationAvecNote.Notes.ElementAt(postulationAvecNote.Notes.Count - 1);
            
            //Alors
            postulationAvecNote.Should().NotBeNull();
            postulationAvecNote.Notes.Should().NotBeNull();
            note.Should().NotBeNull();
            note.NotePostulation.Should().Contain("Salaire inférieur à la norme");
        }

        [Fact]
        public void AjouterNote_PretentionSalarialeEntre20000Et39999_GenereNoteSalaireProcheMaisInferieureNorme()
        {
            //Etant donné

            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.PretentionSalariale = 25000;

            //Initialiser la classe à tester
            GenererEvaluation genererEvaluation = new GenererEvaluation();

            //Lorsque
            var postulationAvecNote = genererEvaluation.AjouterNote(postulation);
            var note = postulationAvecNote.Notes.ElementAt(postulationAvecNote.Notes.Count - 1);

            //Alors
            postulationAvecNote.Should().NotBeNull();
            postulationAvecNote.Notes.Should().NotBeNull();
            note.Should().NotBeNull();
            note.NotePostulation.Should().Contain("Salaire proche mais inférieur à la norme");
        }

        [Fact]
        public void AjouterNote_PretentionSalarialeEntre40000Et79999_GenereNoteSalaireDansLaNorme()
        {
            //Etant donné

            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.PretentionSalariale = 60000;

            //Initialiser la classe à tester
            GenererEvaluation genererEvaluation = new GenererEvaluation();

            //Lorsque
            var postulationAvecNote = genererEvaluation.AjouterNote(postulation);
            var note = postulationAvecNote.Notes.ElementAt(postulationAvecNote.Notes.Count - 1);

            //Alors
            postulationAvecNote.Should().NotBeNull();
            postulationAvecNote.Notes.Should().NotBeNull();
            note.Should().NotBeNull();
            note.NotePostulation.Should().Contain("Salaire dans la norme");
        }

        [Fact]
        public void AjouterNote_PretentionSalarialeEntre80000Et99999_GenereNoteSalaireProcheMaisSuperieurNorme()
        {
            //Etant donné

            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.PretentionSalariale = 85000;

            //Initialiser la classe à tester
            GenererEvaluation genererEvaluation = new GenererEvaluation();

            //Lorsque
            var postulationAvecNote = genererEvaluation.AjouterNote(postulation);
            var note = postulationAvecNote.Notes.ElementAt(postulationAvecNote.Notes.Count - 1);

            //Alors
            postulationAvecNote.Should().NotBeNull();
            postulationAvecNote.Notes.Should().NotBeNull();
            note.Should().NotBeNull();
            note.NotePostulation.Should().Contain("Salaire proche mais supérieur à la norme");
        }

        [Fact]
        public void AjouterNote_PretentionSalarialeSuperieurA99999_GenereNoteSalaireSuperieurNorme()
        {
            //Etant donné

            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.PretentionSalariale = 110000;

            //Initialiser la classe à tester
            GenererEvaluation genererEvaluation = new GenererEvaluation();

            //Lorsque
            var postulationAvecNote = genererEvaluation.AjouterNote(postulation);
            var note = postulationAvecNote.Notes.ElementAt(postulationAvecNote.Notes.Count - 1);

            //Alors
            postulationAvecNote.Should().NotBeNull();
            postulationAvecNote.Notes.Should().NotBeNull();
            note.Should().NotBeNull();
            note.NotePostulation.Should().Contain("Salaire supérieur à la norme");
        }
    }
}
