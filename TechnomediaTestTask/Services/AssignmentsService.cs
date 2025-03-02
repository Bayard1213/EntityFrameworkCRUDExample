using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TechnomediaTestTask.DTOs;
using TechnomediaTestTask.DTOs.Assignment;
using TechnomediaTestTask.DTOs.WorkLog;

namespace TechnomediaTestTask.Services
{
    public class AssignmentsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public AssignmentsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }


        public IEnumerable<AssignmentDTO> GetAllAssignments()
        {
            return _context.assignments
                .Select( a => new AssignmentDTO
                {
                    Id = a.id,
                    TeamId = a.team_id,
                    TeamName = a.teams != null ? a.teams.name : "Unknown",
                    RequestId = a.request_id,
                    RequestNote = a.requests != null ? a.requests.notes : "No notes",
                })
                .ToList();
        }

        public AssignmentDTO GetAssignmentById(int id)
        {
            return _context.assignments.
                Where(a => a.id == id)
                .Select(a => new AssignmentDTO
                {
                    Id = a.id,
                    TeamId = a.team_id,
                    TeamName = a.teams != null ? a.teams.name : "Unknown",
                    RequestId = a.request_id,
                    RequestNote = a.requests != null ? a.requests.notes : "No notes",
                })
                .FirstOrDefault();
        }

        public void CreateAssignment(CreateAssignmentDTO newAssignmentDTO)
        {
            if (!_context.teams.Any(t => t.id == newAssignmentDTO.TeamId))
            {
                throw new ArgumentException("Invalid Team ID");
            }

            if (!_context.requests.Any(r => r.id == newAssignmentDTO.RequestId))
            {
                throw new ArgumentException("Invalid Request ID");
            }

            var newAssignment = new assignments
            {
                team_id = newAssignmentDTO.TeamId,
                request_id = newAssignmentDTO.RequestId,
            };

            _context.assignments.Add(newAssignment);
            _context.SaveChanges();
        }

        public void UpdateAssignment(int id, UpdateAssignmentDTO updatedAssignmentDTO)
        {
            var existingAssignment = _context.assignments.Find(id);

            if (existingAssignment == null)
            {
                throw new ArgumentException("Invalid Assignment ID.");
            }

            if(!_context.teams.Any(t => t.id == updatedAssignmentDTO.TeamId))
            {
                throw new ArgumentException("Invalid Team ID");
            }

            if (!_context.requests.Any(r => r.id == updatedAssignmentDTO.RequestId))
            {
                throw new ArgumentException("Invalid Request ID");
            }

            existingAssignment.team_id = updatedAssignmentDTO.TeamId;
            existingAssignment.request_id = updatedAssignmentDTO.RequestId;

            _context.Entry(existingAssignment).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void DeleteAssignment(int id)
        {
            var assignment = _context.assignments.Find(id);

            if (assignment == null)
            {
                throw new ArgumentException("Invalid Assigment ID.");
            }

            _context.assignments.Remove(assignment);
            _context.SaveChanges();
        }

        public IEnumerable<AssignmentDTO> GetAssignmentsByTeamId(int id)
        {
            return _context.assignments.
                Where(a => a.team_id == id)
                .Select(a => new AssignmentDTO
                {
                    Id = a.id,
                    TeamId = a.team_id,
                    TeamName = a.teams != null ? a.teams.name : "Unknown",
                    RequestId = a.request_id,
                    RequestNote = a.requests != null ? a.requests.notes : "No notes",
                })
                .ToList();
        }
    }
}