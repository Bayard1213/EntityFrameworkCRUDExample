using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TechnomediaTestTask.DTOs.WorkLog;

namespace TechnomediaTestTask.Services
{
    public class WorkLogsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public WorkLogsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<WorkLogDTO> GetAllWorkLogs()
        {
            return _context.work_logs
                .Select(w => new WorkLogDTO
                {
                    Id = w.id,
                    AssignmentId = w.assignment_id,
                    StartTime = w.start_time,
                    EndTime = w.end_time,
                    Comments = w.comments,
                })
                .ToList();
        }

        public WorkLogDTO GetWorkLogById(int id)
        {
            return _context.work_logs.
                Where(w => w.id == id)
                .Select(w => new WorkLogDTO
                {
                    Id = w.id,
                    AssignmentId = w.assignment_id,
                    StartTime = w.start_time,
                    EndTime = w.end_time,
                    Comments = w.comments,
                })
                .FirstOrDefault();
        }

        public void CreateWorkLog(CreateWorkLogDTO newWorkLogDTO)
        {

            if (newWorkLogDTO.EndTime.HasValue && newWorkLogDTO.StartTime.HasValue && newWorkLogDTO.StartTime >= newWorkLogDTO.EndTime)
            {
                throw new ArgumentException("Start time cannot be later than or equal to end time.");
            }

            var assignment = _context.assignments.Include(a => a.requests)
                .FirstOrDefault(a => a.id == newWorkLogDTO.AssignmentId);

            if (assignment == null)
            {
                throw new ArgumentException("Invalid Assignment ID.");
            }

            if (assignment.requests.create_date != null && newWorkLogDTO.StartTime < assignment.requests.create_date)
            {
                throw new ArgumentException("Start time cannot be earlier than request creation date.");
            }

            var newWorkLog = new work_logs
            {
                assignment_id = newWorkLogDTO.AssignmentId,
                start_time = newWorkLogDTO.StartTime,
                end_time = newWorkLogDTO.EndTime,
                comments = newWorkLogDTO.Comments,
            };

            _context.work_logs.Add(newWorkLog);
            _context.SaveChanges();
        }

        public void UpdateWorkLog(int id, UpdateWorkLogDTO updatedWorkLogDTO)
        {
            var existingWorkLog = _context.work_logs.Find(id);
            
            if (existingWorkLog == null)
            {
                throw new ArgumentException("Invalid WorkLog ID.");
            }

            if (updatedWorkLogDTO.EndTime.HasValue && updatedWorkLogDTO.StartTime.HasValue && updatedWorkLogDTO.StartTime >= updatedWorkLogDTO.EndTime)
            {
                throw new ArgumentException("Start time cannot be later than or equal to end time.");
            }

            var assignment = _context.assignments.Include(a => a.requests)
                .FirstOrDefault(a => a.id == updatedWorkLogDTO.AssignmentId);

            if (assignment == null)
            {
                throw new ArgumentException("Invalid Assignment ID.");
            }

            if (assignment.requests.create_date != null && updatedWorkLogDTO.StartTime < assignment.requests.create_date)
            {
                throw new ArgumentException("Start time cannot be earlier than request creation date.");
            }

            existingWorkLog.assignment_id = updatedWorkLogDTO.AssignmentId;
            existingWorkLog.start_time = updatedWorkLogDTO.StartTime;
            existingWorkLog.end_time = updatedWorkLogDTO.EndTime;
            existingWorkLog.comments = updatedWorkLogDTO.Comments;

            _context.Entry(existingWorkLog).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteWorkLog(int id)
        {
            var workLog = _context.work_logs.Find(id);

            if (workLog == null)
            {
                throw new ArgumentException("Invalid WorkLog ID.");
            }

            _context.work_logs.Remove(workLog);
            _context.SaveChanges();
        }

        public IEnumerable<WorkLogDTO> GetWorkLogsByTeamId (int teamId)
        {
            return _context.work_logs
                .Where(w => w.assignments.team_id == teamId)
                .Select(w => new WorkLogDTO
                {
                    Id = w.id,
                    AssignmentId = w.assignment_id,
                    StartTime = w.start_time,
                    EndTime = w.end_time,
                    Comments = w.comments,
                })
                .ToList();
        }

        public IEnumerable<WorkLogDTO> GetWorkLogsByRequestId (int requestId)
        {
            return _context.work_logs
                .Where(w => w.assignments.request_id == requestId)
                .Select(w => new WorkLogDTO
                {
                    Id = w.id,
                    AssignmentId = w.assignment_id,
                    StartTime = w.start_time,
                    EndTime = w.end_time,
                    Comments = w.comments,
                })
                .ToList();
        }
    }
}