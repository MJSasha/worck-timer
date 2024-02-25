using NPOI.SS.UserModel;

namespace WorkTimer.Api.Reports.Xlsx
{
    public interface IExcelReportSheetFiller<T> where T : class
    {
        string SheetName { get; }
        Task FillSheet(ISheet sheet, T config);
    }
}
