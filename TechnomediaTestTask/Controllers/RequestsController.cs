using System;
using System.Linq;
using System.Web.Http;
using TechnomediaTestTask.DTOs.Assignment;
using TechnomediaTestTask.DTOs.Request;
using TechnomediaTestTask.Enums;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/request")]
    public class RequestsController : ApiController
    {
        private readonly RequestsService _requestsService;

        public RequestsController(RequestsService requestsService)
        {
            _requestsService = requestsService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllRequests()
        {
            try
            {
                var requests = _requestsService.GetAllRequests();
                if (!requests.Any())
                {
                    return NotFound();
                }
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{requestId:int}")]
        public IHttpActionResult GetRequestById(int requestId)
        {
            try
            {
                var request = _requestsService.GetRequestById(requestId);

                return Ok(request);
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

        [Authorize(Roles = "Administrator,Secretary")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateRequest(CreateRequestDTO newRequestDTO)
        {
            if (newRequestDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _requestsService.CreateRequest(newRequestDTO);
                return Ok("Request created successfully");
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
        [Route("{requestId:int}")]
        public IHttpActionResult UpdateRequest(int requestId, UpdateRequestDTO updatedRequestDTO)
        {
            if (updatedRequestDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _requestsService.UpdateRequest(requestId, updatedRequestDTO);
                return Ok("Request updated successfully");
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
        [Route("{requestId:int}")]
        public IHttpActionResult DeleteRequest(int requestId)
        {
            try
            {
                _requestsService.DeleteRequest(requestId);
                return Ok("Request deleted successfully.");
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
        [Route("{requestId:int}/complete")]
        public IHttpActionResult ChangeStatusRequestOnCompleted(int requestId, CompleteStatusRequestDTO completeStatusRequestDTO)
        {
            try
            {
                _requestsService.ChangeStatusRequestOnCompleted(requestId, completeStatusRequestDTO);
                return Ok("Request status changed to Completed.");
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