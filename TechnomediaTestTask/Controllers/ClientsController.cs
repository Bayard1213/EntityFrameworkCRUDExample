using System;
using System.Linq;
using System.Web.Http;
using TechnomediaTestTask.DTOs.Client;
using TechnomediaTestTask.DTOs.WorkLog;
using TechnomediaTestTask.Services;

namespace TechnomediaTestTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/client")]
    public class ClientsController : ApiController
    {
        private readonly ClientsService _clientsService;

        public ClientsController(ClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllClients()
        {
            try
            {
                var clients = _clientsService.GetAllClients();
                if (!clients.Any())
                {
                    return NotFound();
                }
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{clientId:int}")]
        public IHttpActionResult GetClientById(int clientId)
        {
            try
            {
                var client = _clientsService.GetClientById(clientId);
                return Ok(client);
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

        [Authorize(Roles = "Secretary,Administrator")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateClient(CreateClientDTO newClientDTO)
        {
            if (newClientDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _clientsService.CreateClient(newClientDTO);
                return Ok("Client created successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Secretary,Administrator")]
        [HttpPut]
        [Route("{clientId:int}")]
        public IHttpActionResult UpdateClient(int clientId, UpdateClientDTO updatedClientDTO)
        {
            if (updatedClientDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                _clientsService.UpdateClient(clientId, updatedClientDTO);
                return Ok("Client updated successfully.");
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

        [Authorize(Roles = "Secretary,Administrator")]
        [HttpDelete]
        [Route("{clientId:int}")]
        public IHttpActionResult DeleteClient(int clientId)
        {
            try
            {
                _clientsService.DeleteClient(clientId);
                return Ok("Client deleted successfully.");
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