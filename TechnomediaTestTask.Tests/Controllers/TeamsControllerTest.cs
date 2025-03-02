using Moq;
using NUnit.Framework;
using TechnomediaTestTask.DTOs;
using TechnomediaTestTask.Services;
using TechnomediaTestTask.Controllers;
using System.Web.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Data.Entity;

namespace TechnomediaTestTask.Tests.Controllers
{
    [TestFixture]
    public class TeamsControllerTest
    {
        private Mock<TechnomediaTestTaskDBEntities> _mockContext;
        private Mock<DbSet<teams>> _mockSet;
        private TeamsService _teamsService;
        private TeamsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockSet = new Mock<DbSet<teams>>();

            var data = new List<teams>
        {
            new teams { id = 1, name = "Team 1" },
            new teams { id = 2, name = "Team 2" }
        }.AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext = new Mock<TechnomediaTestTaskDBEntities>();
            _mockContext.Setup(m => m.teams).Returns(_mockSet.Object);

            _teamsService = new TeamsService(_mockContext.Object);
            _controller = new TeamsController(_teamsService);
        }

        #region GetAllTeam
        [Test]
        public void GetAllTeams_ReturnsOk_WhenTeamsExist()
        {
            var result = _controller.GetAllTeams() as OkNegotiatedContentResult<IEnumerable<TeamDTO>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Content.Count());
        }

        [Test]
        public void GetAllTeams_ReturnsNotFound_WhenNoTeamsExist()
        {
            var data = new List<teams>().AsQueryable(); 
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.teams).Returns(_mockSet.Object);

            var result = _controller.GetAllTeams() as NotFoundResult;

            Assert.IsNotNull(result);
        }
        #endregion

        #region GetTeam
        [Test]
        public void GetTeam_ReturnsOk_WhenTeamExists()
        {
            var teamId = 1;
            var mockTeam = new teams { id = teamId, name = "Team 1" };

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(mockTeam);

            var result = _controller.GetTeamById(teamId) as OkNegotiatedContentResult<TeamDTO>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(teamId, result.Content.Id);
        }

        [Test]
        public void GetTeam_ReturnsNotFound_WhenTeamDoesNotExist()
        {
            var teamId = 3;

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((teams)null);

            var result = _controller.GetTeamById(teamId) as NotFoundResult;

            Assert.IsNotNull(result);
        }
        #endregion

        #region CreateTeam
        [Test]
        public void CreateTeam_ReturnsOk_WhenDataIsValid()
        {
            var validTeamDTO = new CreateTeamDTO { Name = "Valid Team" };

            var result = _controller.CreateTeam(validTeamDTO) as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual("Team created successfully.", result.Content);
        }

        [Test]
        public void CreateTeam_ReturnsBadRequest_WhenDataIsInvalid()
        {
            CreateTeamDTO invalidTeamDTO = null;

            var result = _controller.CreateTeam(invalidTeamDTO) as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid data.", result.Message);
        }
        #endregion

        #region UpdateTeam
        [Test]
        public void UpdateTeam_ReturnsBadRequest_WhenDataIsInvalid()
        {
            UpdateTeamDTO invalidTeamDTO = null;

            var result = _controller.UpdateTeam(1, invalidTeamDTO) as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid data.", result.Message);
        }

        [Test]
        public void UpdateTeam_ReturnsNotFound_WhenTeamDoesNotExist()
        {
            var teamId = 1;
            var updatedTeamDTO = new UpdateTeamDTO { Name = "Updated Team Name" };

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((teams)null);

            var result = _controller.UpdateTeam(teamId, updatedTeamDTO) as NotFoundResult;

            Assert.IsNotNull(result);
        }
        #endregion

        #region DeleteTeam
        [Test]
        public void DeleteTeam_ReturnsOk_WhenTeamIsDeletedSuccessfully()
        {
            var teamId = 1;
            var existingTeam = new teams { id = teamId, name = "Team 1" };

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(existingTeam);
            _mockSet.Setup(m => m.Remove(It.IsAny<teams>())).Callback<teams>(t => existingTeam = null);

            var result = _controller.DeleteTeam(teamId) as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual("Team deleted successfully.", result.Content);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void DeleteTeam_ReturnsNotFound_WhenTeamDoesNotExist()
        {
            var teamId = 1;

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((teams)null);

            var result = _controller.DeleteTeam(teamId) as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Team ID.", result.Message);
        }
        #endregion

    }
}
