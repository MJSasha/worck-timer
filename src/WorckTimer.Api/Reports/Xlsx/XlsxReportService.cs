using NPOI.XSSF.UserModel;

namespace WorkTimer.Api.Reports.Xlsx
{
    public class XlsxReportService<T> where T : class
    {
        private readonly IEnumerable<IExcelReportSheetFiller<T>> excelReportSheetFillers;

        public XlsxReportService(IEnumerable<IExcelReportSheetFiller<T>> excelReportSheetFillers)
        {
            this.excelReportSheetFillers = excelReportSheetFillers;
        }

        public async Task<MemoryStream> CreateReport(T config)
        {
            var xlsx = new XSSFWorkbook();

            foreach (var filler in excelReportSheetFillers)
            {
                var sheet = xlsx.CreateSheet(filler.SheetName);
                await filler.FillSheet(sheet, config);
            }

            var stream = new MemoryStream();
            xlsx.Write(stream, true);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }

}
