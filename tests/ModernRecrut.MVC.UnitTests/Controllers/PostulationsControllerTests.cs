using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using ModernRecrut.MVC.Controllers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.Log;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace ModernRecrut.MVC.UnitTests.Controllers
{
    public class PostulationsControllerTests
    {
        [Fact]
        public async Task Postuler_CandidatNePossedeLettreEtCv_Retourne_ViewResultAvecModelError()
        {
            //Etant donné
            //Initialisation de la postulation
            var fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();

            //Initialisation de l'instance de Mock
            var mockPostulationService = new Mock<IPostulationService>();
            var mockLoggerService = new Mock<ILogger<PostulationsController>>();
            mockPostulationService.Setup(e => e.Ajouter(It.IsAny<Postulation>()));

            var postulationsController = new PostulationsController(mockLoggerService.Object, mockPostulationService.Object);
            

            //Lorsque
            var actionResult = await postulationsController.Postuler(postulation) as ViewResult;
            var modelState = postulationsController.ModelState;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);

            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(2);
            modelState[""].Errors.ElementAt(0).ErrorMessage.Should().Be("Vous devez déposer une lettre pour Postuler");
            modelState[""].Errors.ElementAt(1).ErrorMessage.Should().Be("Vous devez déposer un CV pour Postuler");
            
            mockPostulationService.Verify(e => e.Ajouter(It.IsAny<Postulation>()), Times.Never);
        }

        [Fact]
        public async void Postuler_Post_Postulation_Retourne_RedirectToAction_Index()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.DateDisponibilite = DateTime.Now.AddDays(15);
            //postulation.PretentionSalariale = 120000;
            //postulation.IdCandidat = "9f4148a0-a7be-4536-8732-8816b198b0ed";

            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();

            postulationService.Setup(p => p.CandidatPossedeCV(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.CandidatPossedeLettre(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.Ajouter(It.IsAny<Postulation>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Postuler(postulation) as RedirectToActionResult;

            //Alors
            actionResult.Should().NotBeNull();
            actionResult.ActionName.Should().Be("Index");
            postulationService.Verify(p => p.Ajouter(It.IsAny<Postulation>()), Times.Once);

        }
        [Fact]
        public async void Postuler_Post_PretentionTropElever_Retourne_ViewResultAvecModelError()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.DateDisponibilite = DateTime.Now.AddDays(15);

            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();

            postulationService.Setup(p => p.PretentionsSalarialeTropElever(It.IsAny<decimal>())).Returns(true);
            postulationService.Setup(p => p.CandidatPossedeCV(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.CandidatPossedeLettre(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.Ajouter(It.IsAny<Postulation>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Postuler(postulation) as ViewResult;
            var modelState = controller.ModelState;
            //Alors
            actionResult.Should().NotBeNull();

            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState["PretentionSalariale"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Votre prétention salariale est au-delà de nos limites");

            postulationService.Verify(p => p.Ajouter(It.IsAny<Postulation>()), Times.Never);
        }
        [Fact]
        public async void Postuler_Post_MauvaiseDateDisponibilite_Retourne_ViewResultAvecModelError()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();

            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();

            postulationService.Setup(p => p.DateDisponibiliteIncorecte(It.IsAny<DateTime>())).Returns(true);
            postulationService.Setup(p => p.CandidatPossedeCV(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.CandidatPossedeLettre(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            postulationService.Setup(p => p.Ajouter(It.IsAny<Postulation>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Postuler(postulation) as ViewResult;
            var modelState = controller.ModelState;
            //Alors
            actionResult.Should().NotBeNull();

            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState[""].Errors.FirstOrDefault().ErrorMessage.Should().Be($"La date de disponibilité doit être supérieure à {DateTime.Now:MM/dd/yyyy} et inférieure au {DateTime.Now.AddDays(45):MM/dd/yyyy}");

            postulationService.Verify(p => p.Ajouter(It.IsAny<Postulation>()), Times.Never);
        }

        [Fact]
        public async void Edit_Get_id_Retourne_ViewResultAvecModelPostulation()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation.Id = 0;
            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();

            
            postulationService.Setup(p => p.ObtenirSelonId(It.IsAny<int>())).Returns(() => Task.FromResult(postulation));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Edit(0) as ViewResult;
            var model = actionResult.Model;
            //Alors
            model.Should().NotBeNull();
            actionResult.Should().NotBeNull();

        }
        [Fact]
        public async void Edit_Get_id_Retourne_NotFound()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation = null;
            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();


            postulationService.Setup(p => p.ObtenirSelonId(It.IsAny<int>())).Returns(() => Task.FromResult(postulation));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Edit(0) as NotFoundResult;

            //Alors
            actionResult.Should().NotBeNull();

        }
        [Fact]
        public async void Edit_Post_Postulation_Retourne_NotFound()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            postulation = null;
            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();


            postulationService.Setup(p => p.ObtenirSelonId(It.IsAny<int>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Edit(postulation) as NotFoundResult;

            //Alors
            actionResult.Should().NotBeNull();

        }
        [Fact]
        public async void Edit_Post_Postulation_Retourne_RedirectToActionListePostulations()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();


            postulationService.Setup(p => p.Modifier(It.IsAny<Postulation>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Edit(postulation) as RedirectToActionResult;

            //Alors
            actionResult.Should().NotBeNull();
            actionResult.ActionName.Should().Be("ListePostulations");
            postulationService.Verify(p => p.Modifier(It.IsAny<Postulation>()), Times.Once);
        }
        [Fact]
        public async void Edit_Post_Postulation_MauvaiseDatePourModification_Retourne_ViewResultAvecModelError()
        {
            //Etant donné
            //Initialisation de la postulation
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            //Initialisation de l'instance de Mock
            var postulationService = new Mock<IPostulationService>();
            var logger = new Mock<ILogger<PostulationsController>>();

            postulationService.Setup(p => p.DateIncorectePour_Modification_Supression(It.IsAny<DateTime>())).Returns(true);
            postulationService.Setup(p => p.Modifier(It.IsAny<Postulation>()));

            var controller = new PostulationsController(logger.Object, postulationService.Object);

            //Lorsque
            var actionResult = await controller.Edit(postulation) as ViewResult;
            var model = actionResult.Model;
            var modelState = controller.ModelState;
            //Alors
            actionResult.Should().NotBeNull();

            model.Should().NotBeNull();
            model.Should().Be(postulation);
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState[""].Errors.FirstOrDefault().ErrorMessage.Should().Be("La postulation ne peut pas être modifiée car la date de l'entrevue est proche.");

            postulationService.Verify(p => p.Modifier(It.IsAny<Postulation>()), Times.Never);
        }
    }
}