using System;
using System.Linq;
using System.Web.Http;
using TechnomediaTestTask.DTOs.Assignment;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/assignment")]
    public class AssignmentsController : ApiController
    {
        private readonly AssignmentsService _assignmentsService;

        public AssignmentsController(AssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllAssignments()
        {
            try
            {
                var assignments = _assignmentsService.GetAllAssignments();
                if (!assignments.Any())
                {
                    return NotFound();
                }
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{assignmentId:int}")]
        public IHttpActionResult GetAssignmentById(int assignmentId)
        {
            try
            {
                var assignment = _assignmentsService.GetAssignmentById(assignmentId);
                return Ok(assignment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Secretary")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateAssignment(CreateAssignmentDTO newAssignmentDTO)
        {
            if (newAssignmentDTO == null) 
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _assignmentsService.CreateAssignment(newAssignmentDTO);
                return Ok("Assignment created successfully");
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

        [Authorize(Roles = "Administrator,Secretary")]
        [HttpPut]
        [Route("{assignmentId:int}")]
        public IHttpActionResult UpdateAssignment(int assignmentId, UpdateAssignmentDTO updatedAssignmentDTO)
        {
            if (updatedAssignmentDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _assignmentsService.UpdateAssignment(assignmentId, updatedAssignmentDTO);
                return Ok("Assignment updated successfully");
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

        [Authorize(Roles = "Administrator,Secretary")]
        [HttpDelete]
        [Route("{assignmentId:int}")]
        public IHttpActionResult DeleteAssignment(int assignmentId)
        {
            try
            {
                _assignmentsService.DeleteAssignment(assignmentId);
                return Ok("Assignment deleted successfully.");
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

        [Authorize(Roles = "Administrator,Worker")]
        [HttpGet]
        [Route("team/{teamId:int}")]
        public IHttpActionResult GetAssignmentsByTeamId (int teamId)
        {
            try
            {
                var assignments = _assignmentsService.GetAssignmentsByTeamId(teamId);
                if (!assignments.Any())
                {
                    return NotFound();
                }
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}