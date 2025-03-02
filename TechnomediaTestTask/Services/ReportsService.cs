using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TechnomediaTestTask.DTOs.Report;
using TechnomediaTestTask.Enums;

namespace TechnomediaTestTask.Services
{
    public class ReportsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public ReportsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }

        public ReportDTO GetMonthlyReportByTeam(Month month, int year, int teamId)
        {
            try
            {
                var startOfMonth = new DateTime(year, (int)month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var workLogs = _context.work_logs
                    .Where(wl => wl.assignments.team_id == teamId && wl.start_time >= startOfMonth && wl.end_time <= endOfMonth)
                    .AsEnumerable();

                if (!workLogs.Any())
                {
                    return null;
                }

                var report = workLogs
                    .GroupBy(wl => wl.assignments.team_id)
                    .Select(g => new ReportDTO
                    {
                        TeamId = g.Key,
                        SelectedMonth = month,
                        TeamName = g.FirstOrDefault()?.assignments?.teams?.name ?? "Unknown",
                        CompletedRequests = g.Select(wl => wl.assignments.requests)
                            .Where(r => r.status == RequestStatus.Completed.ToString()).Count(),
                        TotalTimeSpent = (int)Math.Round(g.Sum(wl =>
                        {
                            var actualStart = (wl.start_time < startOfMonth) ? startOfMonth : wl.start_time.Value;
                            var actualEnd = (wl.end_time > endOfMonth) ? endOfMonth : wl.end_time.Value;

                            return (actualEnd > actualStart) ? (actualEnd - actualStart).TotalHours : 0;
                        }))
                    })
                    .SingleOrDefault();

                return report;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Could not generate report due to invalid state of data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while generating the report.", ex);
            }
        }

        public IEnumerable<ReportDTO> GetMonthlyReport(Month month, int year)
        {
            try
            {
                var startOfMonth = new DateTime(year, (int)month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                var workLogs = _context.work_logs
                    .Where(wl => wl.start_time < endOfMonth && wl.end_time >= startOfMonth)
                    .AsEnumerable();

                if (!workLogs.Any())
                {
                    return Enumerable.Empty<ReportDTO>(); 
                }

                var reports = workLogs
                    .GroupBy(wl => wl.assignments.team_id)
                    .Select(g => new ReportDTO
                    {
                        TeamId = g.Key,
                        SelectedMonth = month,
                        TeamName = g.FirstOrDefault()?.assignments?.teams?.name ?? "Unknown",
                        CompletedRequests = g.Select(wl => wl.assignments.requests)
                            .Where(r => r.status == RequestStatus.Completed.ToString()).Count(),
                        TotalTimeSpent = (int)Math.Round(g.Sum(wl =>
                        {
                            var actualStart = (wl.start_time < startOfMonth) ? startOfMonth : wl.start_time.Value;
                            var actualEnd = (wl.end_time > endOfMonth) ? endOfMonth : wl.end_time.Value;

                            return (actualEnd > actualStart) ? (actualEnd - actualStart).TotalHours : 0;
                        }))
                    })
                    .ToList();

                return reports;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Could not generate report due to invalid state of data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while generating the report.", ex);
            }
        }
    }
}