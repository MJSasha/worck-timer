using MathNet.Numerics;
using NPOI.SS.UserModel;
using WorkTimer.Api.Reports.Xlsx.Configurations;
using WorkTimer.Api.Reports.Xlsx.Definitions;
using WorkTimer.Api.Services;

namespace WorkTimer.Api.Reports.Xlsx.ReportsScheetsFillers
{
    public class UsersSheetFiller : IExcelReportSheetFiller<UsersReportConfig>
    {
        public string SheetName => "Пользователи";

        private readonly WorkPeriodsService workPeriodsService;
        private readonly ReportStyle reportStyle;

        public UsersSheetFiller(WorkPeriodsService workPeriodsService, ReportStyle reportStyle)
        {
            this.workPeriodsService = workPeriodsService;
            this.reportStyle = reportStyle;
        }


        public async Task FillSheet(ISheet sheet, UsersReportConfig config)
        {
            var reportData = await workPeriodsService.GetUsersWorksDurationsReportByMonth(config.StartDate, config.EndDate);

            var reportBuilder = new XlsxReportBuilder(sheet);
            AddSheetTitle(reportBuilder);

            var cellStyle = reportBuilder.CreateCellStyle(reportStyle.AlignCenter with { Alignment = HorizontalAlignment.Left, IsWrapText = true });
            var counterStyle = reportBuilder.CreateCellStyle(reportStyle.AlignCenter);
            foreach (var monthRow in reportData)
            {
                foreach (var usereRow in monthRow.UsersWorksDurationsInfos)
                {
                    reportBuilder.AddRow(row =>
                    {
                        int columnIndex = 0;

                        reportBuilder.SetCellValue(row, columnIndex++, value: $"{monthRow.Month}-{monthRow.Year}", cellType: CellType.String, cellStyle: counterStyle);
                        reportBuilder.SetCellValue(row, columnIndex++, value: usereRow.User.Name, cellType: CellType.String, cellStyle: cellStyle);
                        reportBuilder.SetCellValue(row, columnIndex++, value: usereRow.WorkDuration.TotalHours, cellType: CellType.Numeric, cellStyle: counterStyle);
                        reportBuilder.SetCellValue(row, columnIndex++, value: usereRow.TotalSalary.Round(2), cellType: CellType.Numeric, cellStyle: counterStyle);
                    });
                }

                if (monthRow.UsersWorksDurationsInfos.Count > 1)
                {
                    reportBuilder.AddMergedRegion(reportBuilder.CurrentRow - monthRow.UsersWorksDurationsInfos.Count, reportBuilder.CurrentRow - 1, 0, 0);
                }
            }
        }

        private void AddSheetTitle(XlsxReportBuilder reportBuilder)
        {
            reportBuilder.CreateFreezePane(0, 2);
            reportBuilder.AddTitle(SheetName, colSpan: 4, cellStyle: reportBuilder.CreateCellStyle(reportStyle.Title));

            reportBuilder.AddRow(
                reportBuilder.CreateCellStyle(reportStyle.FirstHeader),
                "Дата",
                "Сотрудник",
                "Вемя работы",
                "Зарплата"
            );
        }

    }
}
