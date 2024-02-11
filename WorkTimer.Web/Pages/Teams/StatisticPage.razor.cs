using Microsoft.AspNetCore.Components;
using MudBlazor;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;
using WorkTimer.Web.Common.Utils;

namespace WorkTimer.Web.Pages.Teams
{
    public partial class StatisticPage : ComponentBase
    {
        [Inject]
        public IWorkPeriod WorkPeriodsService { get; set; }

        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }
        private List<UsersWorksDurationsReportByMonth> usersWorksDurationsReports;
        private List<StatisticWrapper> statistic;

        private bool isLoading;
        private ChartOptions options = new();
        private List<ChartSeries> series = new();
        private string[] xAxisLabels;
        private string[] donutLabels;
        private int donutSelectedIndex = -1;
        private double[] donutData;

        private TableGroupDefinition<StatisticWrapper> groupDefinition = new()
        {
            GroupName = "Group",
            Indentation = false,
            Expandable = false,
            Selector = (e) => e.MonthName
        };

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await base.OnInitializedAsync();

            await LoadStatistic();
            FormatChartStatistic();
            IsLoading = false;
        }

        private async Task LoadStatistic()
        {
            usersWorksDurationsReports = await WorkPeriodsService.GetUsersWorksDurationsReportByMonth(DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow);

            statistic = usersWorksDurationsReports.SelectMany(s =>
            {
                var monthName = Formatters.GetMonthAndYearNames(s.Year, s.Month);
                return s.UsersWorksDurationsInfos.Select(di => new StatisticWrapper
                {
                    MonthName = monthName,
                    User = di.User,
                    WorkDuration = di.WorkDuration,
                });
            }).ToList();
        }

        private void FormatChartStatistic()
        {
            xAxisLabels = usersWorksDurationsReports.Select(r => $"{r.Month}/{r.Year}").ToArray();

            var groupedReports = usersWorksDurationsReports
                .SelectMany(report => report.UsersWorksDurationsInfos)
                .GroupBy(info => new { info.User.Id, info.User.Name });

            foreach (var userGroup in groupedReports)
            {
                string userName = userGroup.Key.Name;
                double[] durations = userGroup.Select(info => info.WorkDuration.TotalHours).ToArray();

                series.Add(new ChartSeries() { Name = userName, Data = durations });
            }

            donutLabels = series.Select(s => s.Name).ToArray();
            donutData = series.Select(s => s.Data.Sum()).ToArray();
        }

        private sealed class StatisticWrapper : UserWorkDurationInfo
        {
            public string MonthName { get; set; }
        }
    }
}
