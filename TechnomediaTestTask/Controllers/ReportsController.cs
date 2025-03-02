using System.Linq;
using System;
using System.Web.Http;
using TechnomediaTestTask.Enums;
using TechnomediaTestTask.Services;
using System.Net;

namespace TechnomediaTestTask.Controllers
{
    [Authorize(Roles = "Director,Administrator")]
    [RoutePrefix("api/report")]
    public class ReportsController : ApiController
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet]
        [Route("monthly/{month}/{year:int}")]
        public IHttpActionResult GetMonthlyReport(Month month, int year)
        {
            try
            {
                var reports = _reportsService.GetMonthlyReport((Month)month, year);
                if (reports == null || !reports.Any())
                {
                    return Ok("No work logs found for the given time period.");
                }
                return Ok(reports);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message); 
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        [HttpGet]
        [Route("monthly/{month}/{year:int}/team/{teamId:int}")]
        public IHttpActionResult GetMonthlyReportByTeam(Month month, int year, int teamId)
        {
            try
            {
                var report = _reportsService.GetMonthlyReportByTeam((Month)month, year, teamId);
                if (report == null )
                {
                    return Ok("No work logs found for the specified team and time period.");
                }
                return Ok(report); 
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message); 
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }
    }
}