using Moq;
using NUnit.Framework;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using TechnomediaTestTask.Services;
using TechnomediaTestTask.DTOs;
using System;
using System.Data.Entity.Infrastructure;
using Microsoft.Testing.Platform.Extensions.Messages;

namespace TechnomediaTestTask.Tests.Services
{
    [TestFixture]
    public class TeamsServiceTest
    {
        private Mock<DbSet<teams>> _mockSet;
        private Mock<TechnomediaTestTaskDBEntities> _mockContext;
        private TeamsService _teamsService;

        [SetUp]
        public void SetUp()
        {
            _mockSet = new Mock<DbSet<teams>>();
            _mockContext = new Mock<TechnomediaTestTaskDBEntities>();
            _mockContext.Setup(m => m.teams).Returns(_mockSet.Object);

            var data = new List<teams>
            {
                new teams { id = 1, name = "Team 1" },
                new teams { id = 2, name = "Team 2" }
            }.AsQueryable();

            _mockSet = new Mock<DbSet<teams>>();
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext = new Mock<TechnomediaTestTaskDBEntities>();
            _mockContext.Setup(c => c.teams).Returns(_mockSet.Object);

            _teamsService = new TeamsService(_mockContext.Object);
        }

        #region GetAllTeams
        [Test]
        public void GetAllTeams_ReturnsAllTeams_WhenTeamsExist()
        {
            var result = _teamsService.GetAllTeams();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Team 1", result.First().Name);
        }

        [Test]
        public void GetAllTeams_ReturnsEmptyList_WhenNoTeamsExist()
        {
            var emptyData = new List<teams>().AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(emptyData.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(emptyData.GetEnumerator());

            var result = _teamsService.GetAllTeams();

            Assert.IsEmpty(result);
        }
        #endregion

        #region GetTeamById
        [Test]
        public void GetTeamById_ReturnsCorrectTeam_WhenTeamExists()
        {
            var result = _teamsService.GetTeamById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Team 1", result.Name);
        }

        [Test]
        public void GetTeamById_ReturnsNull_WhenTeamDoesNotExist()
        {
            var result = _teamsService.GetTeamById(3); 

            Assert.IsNull(result);
        }
        #endregion

        #region CreateTeam
        [Test]
        public void CreateTeam_AddsNewTeam_WhenCalledWithValidData()
        {
            var newTeamDTO = new CreateTeamDTO { Name = "New Team" };
            var data = new List<teams>().AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _mockSet.Setup(m => m.Add(It.IsAny<teams>())).Callback<teams>((s) => data.ToList().Add(s));

            _teamsService.CreateTeam(newTeamDTO);

            _mockSet.Verify(m => m.Add(It.IsAny<teams>()), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Test]
        public void CreateTeam_ThrowsException_WhenCalledWithInvalidData()
        {
            var newTeamDTO = new CreateTeamDTO { Name = null }; 

            Assert.Throws<ArgumentException>(() => _teamsService.CreateTeam(newTeamDTO));
        }
        #endregion

        #region UpdateTeam
        [Test]
        public void UpdateTeam_UpdatesExistingTeam_WhenTeamExists()
        {
            var existingTeam = new teams { id = 1, name = "Old Team Name" };
            var data = new List<teams> { existingTeam }.AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.SingleOrDefault(d => d.id == (int)ids[0]));

            var updatedTeamDTO = new UpdateTeamDTO { Name = "New Team Name" };

            var service = new Mock<TeamsService>(_mockContext.Object);
            service.Setup(s => s.SetEntityStateModified(It.IsAny<teams>())).Callback<teams>((t) => { });

            service.Object.UpdateTeam(1, updatedTeamDTO);

            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual("New Team Name", existingTeam.name);
        }

        [Test]
        public void UpdateTeam_ThrowsException_WhenTeamDoesNotExist()
        {
            var data = new List<teams>().AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((teams)null);

            var updatedTeamDTO = new UpdateTeamDTO { Name = "New Team Name" };

            Assert.Throws<ArgumentException>(() => _teamsService.UpdateTeam(1, updatedTeamDTO));
        }
        #endregion

        #region DeleteTeam
        [Test]
        public void DeleteTeam_RemovesTeam_WhenTeamExists()
        {
            var existingTeam = new teams { id = 1, name = "Team 1" };
            var data = new List<teams> { existingTeam }.AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.id == (int)ids[0]));
            _mockSet.Setup(m => m.Remove(It.IsAny<teams>())).Callback<teams>(t => data.ToList().Remove(t));

            _teamsService.DeleteTeam(1);

            _mockSet.Verify(m => m.Remove(It.IsAny<teams>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void DeleteTeam_ThrowsException_WhenTeamDoesNotExist()
        {
            var data = new List<teams>().AsQueryable();

            _mockSet.As<IQueryable<teams>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<teams>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((teams)null);

            Assert.Throws<ArgumentException>(() => _teamsService.DeleteTeam(1));
        }
        #endregion

    }
}
