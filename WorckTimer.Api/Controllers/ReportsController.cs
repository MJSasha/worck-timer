using Microsoft.AspNetCore.Mvc;
using QuickActions.Api.Identity.IdentityCheck;
using WorkTimer.Api.Reports.Xlsx;
using WorkTimer.Api.Reports.Xlsx.Configurations;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly XlsxReportService<UsersReportConfig> reportService;

        public ReportsController(XlsxReportService<UsersReportConfig> reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stream = await reportService.CreateReport(new UsersReportConfig
            {
                StartDate = startDate,
                EndDate = endDate,
            });
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет по пользователям.xlsx");
        }
    }
}
