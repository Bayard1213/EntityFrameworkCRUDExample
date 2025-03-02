using System;
using System.Linq;
using System.Web.Http;
using TechnomediaTestTask.DTOs;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/team")]
    public class TeamsController : ApiController
    {
        private readonly TeamsService _teamsService;

        public TeamsController(TeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllTeams()
        {
            try
            {
                var teams = _teamsService.GetAllTeams();
                if (!teams.Any())
                {
                    return NotFound();
                }
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{teamId:int}")]
        public IHttpActionResult GetTeamById(int teamId)
        {
            try
            {
                var team = _teamsService.GetTeamById(teamId);
                if (team == null)
                {
                    return NotFound();
                }
                return Ok(team);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Director")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateTeam(CreateTeamDTO newTeamDTO)
        {
            if (newTeamDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _teamsService.CreateTeam(newTeamDTO);
                return Ok("Team created successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Director")]
        [HttpPut]
        [Route("{teamId:int}")]
        public IHttpActionResult UpdateTeam(int teamId, UpdateTeamDTO updatedTeamDTO)
        {
            if (updatedTeamDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _teamsService.UpdateTeam(teamId, updatedTeamDTO);
                return Ok("Team updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Director")]
        [HttpDelete]
        [Route("{teamId:int}")]
        public IHttpActionResult DeleteTeam(int teamId)
        {
            try
            {
                _teamsService.DeleteTeam(teamId);
                return Ok("Team deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}