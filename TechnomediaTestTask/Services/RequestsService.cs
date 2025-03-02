using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TechnomediaTestTask.DTOs.Request;
using TechnomediaTestTask.Enums;

namespace TechnomediaTestTask.Services
{
    public class RequestsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public RequestsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<RequestDTO> GetAllRequests()
        {
            return _context.requests
                .AsEnumerable()
                .Select(r => new RequestDTO
                {
                    Id = r.id,
                    ClientId = r.client_id,
                    ClientName = r.clients != null ? r.clients.name : "Unknown",
                    ResearchNotes = r.research_notes,
                    CreateDate = r.create_date,
                    Notes = r.notes,
                    Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), r.status)

                })
                .ToList();
        }

        public RequestDTO GetRequestById(int id)
        {
            return _context.requests.
                Where(r => r.id == id)
                .AsEnumerable()
                .Select(r => new RequestDTO
                {
                    Id = r.id,
                    ClientId = r.client_id,
                    ClientName = r.clients != null ? r.clients.name : "Unknown",
                    ResearchNotes = r.research_notes,
                    CreateDate = r.create_date,
                    Notes = r.notes,
                    Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), r.status)
                })
                .FirstOrDefault();
        }

        public void CreateRequest(CreateRequestDTO newRequestDTO)
        {
            if (!_context.clients.Any(c => c.id == newRequestDTO.ClientId))
            {
                throw new ArgumentException("Invalid Client ID");
            }

            var newRequest = new requests
            {
                client_id = newRequestDTO.ClientId,
                create_date = System.DateTime.Now,
                research_notes = newRequestDTO.ResearchNotes,
                notes = newRequestDTO.Notes,
                status = newRequestDTO.Status.ToString(),
            };

            _context.requests.Add(newRequest);
            _context.SaveChanges();
        }

        public void UpdateRequest(int id, UpdateRequestDTO updatedRequestDTO)
        {
            var existingRequest = _context.requests.Find(id);
            if (existingRequest == null)
            {
                throw new ArgumentException("Invalid Request ID");
            }

            existingRequest.client_id = updatedRequestDTO.ClientId;
            existingRequest.research_notes = updatedRequestDTO.ResearchNotes;
            existingRequest.notes = updatedRequestDTO.Notes;
            existingRequest.status = updatedRequestDTO.Status.ToString();

            _context.Entry(existingRequest).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRequest(int id)
        {
            var request = _context.requests.Find(id);
            if (request == null)
            {
                throw new ArgumentException("Invalid Request ID");
            }

            _context.requests.Remove(request);
            _context.SaveChanges();
        }

        public void ChangeStatusRequestOnCompleted(int id, CompleteStatusRequestDTO completeStatusRequestDTO)
        {
            var existingRequest = _context.requests.Find(id);
            if (existingRequest == null)
            {
                throw new ArgumentException("Invalid Request ID");
            }
            
            existingRequest.status = RequestStatus.Completed.ToString();
            existingRequest.notes = completeStatusRequestDTO.Note;
            existingRequest.research_notes = completeStatusRequestDTO.ResearchNotes;

            _context.Entry(existingRequest).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}