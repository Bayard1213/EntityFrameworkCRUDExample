using System;
using System.Linq;
using System.Web.Http;
using TechnomediaTestTask.DTOs.WorkLog;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/workLogs")]
    public class WorkLogsController : ApiController
    {
        private readonly WorkLogsService _workLogsService;

        public WorkLogsController(WorkLogsService workLogsService)
        {
            _workLogsService = workLogsService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllWorkLogs()
        {
            try
            {
                var workLogs = _workLogsService.GetAllWorkLogs();
                if (!workLogs.Any())
                {
                    return NotFound();
                }
                return Ok(workLogs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{workLogId:int}")]
        public IHttpActionResult GetWorkLogById(int workLogId)
        { 
            try
            {
                var workLogs = _workLogsService.GetWorkLogById(workLogId);
                return Ok(workLogs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Worker")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateWorkLog(CreateWorkLogDTO newWorkLogDTO)
        {
            if (newWorkLogDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _workLogsService.CreateWorkLog(newWorkLogDTO);
                return Ok("Work log created successfully.");
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
        [HttpPut]
        [Route("{workLogId:int}")]
        public IHttpActionResult UpdateWorkLog(int workLogId, UpdateWorkLogDTO updatedWorkLogDTO)
        {
            if (updatedWorkLogDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _workLogsService.UpdateWorkLog(workLogId, updatedWorkLogDTO);
                return Ok("Work log updated successfully.");
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
        [HttpDelete]
        [Route("{workLogId:int}")]
        public IHttpActionResult DeleteWorkLog(int workLogId)
        {
            try
            {
                _workLogsService.DeleteWorkLog(workLogId);
                return Ok("Work log deleted successfully.");
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
        public IHttpActionResult GetWorkLogsByTeamId(int teamId)
        {
            try
            {
                var workLogs = _workLogsService.GetWorkLogsByTeamId(teamId);
                if (!workLogs.Any())
                {
                    return NotFound();
                }
                return Ok(workLogs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Administrator,Worker")]
        [HttpGet]
        [Route("request/{requestId:int}")]
        public IHttpActionResult GetWorkLogsByRequestId(int requestId) 
        {
            try
            {
                var workLogs = _workLogsService.GetWorkLogsByRequestId(requestId);
                if (!workLogs.Any())
                {
                    return NotFound();
                }
                return Ok(workLogs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}