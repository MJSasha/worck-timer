using Microsoft.AspNetCore.Components;
using MudBlazor;
using QuickActions.Common.Data;
using QuickActions.Common.Specifications;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Utils;

namespace WorkTimer.Web.Pages.Teams
{
    public partial class StatisticPage : ComponentBase
    {
        [CascadingParameter]
        public Session<User> CurrentSession { get; set; }

        [Inject]
        public IWorkPeriod WorkPeriodsService { get; set; }

        [Inject]
        public IUsers UsersService { get; set; }

        [Parameter]
        public bool ShowForUser
        {
            get => showForUser;
            set
            {
                if (showForUser != value)
                {
                    showForUser = value;
                    if (showForUser) selectedUser = CurrentSession?.Data;
                    else selectedUser = null;
                    RefreshData();
                }
            }
        }

        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }
        private List<UsersWorksDurationsReportByMonth> usersWorksDurationsReports;
        private List<StatisticWrapper> statistic;
        private List<User> users;

        private DateTime? startDate = DateTime.Now.AddMonths(-3).Date;
        private DateTime? endDate = DateTime.Now.Date;
        private User selectedUser;

        private bool isLoading;
        private bool showDonutChart;
        private bool showForUser;
        private ChartOptions options = new();
        private List<ChartSeries> series = new();
        private string[] xAxisLabels;
        private string[] donutLabels;
        private int donutSelectedIndex = -1;
        private double[] donutData;

        private TableGroupDefinition<StatisticWrapper> groupDefinition = new()
        {
            Indentation = false,
            Expandable = true,
            Selector = (e) => e.MonthWorkInfo
        };

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await RefreshData();
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await base.OnInitializedAsync();

            options.InterpolationOption = InterpolationOption.NaturalSpline;

            if (ShowForUser)
            {
                selectedUser = CurrentSession.Data;
            }
            else
            {
                users = await UsersService.Read(new Specification<User>(), 0, int.MaxValue);
            }

            await RefreshData();
        }

        private async Task RefreshData()
        {
            IsLoading = true;

            await LoadStatistic();
            FormatChartStatistic();

            IsLoading = false;
        }

        private async Task LoadStatistic()
        {
            usersWorksDurationsReports = await WorkPeriodsService.GetUsersWorksDurationsReportByMonth(startDate.Value.ToUniversalTime(), endDate.Value.AddMonths(1).ToUniversalTime(), selectedUser?.Id);

            statistic = usersWorksDurationsReports.SelectMany(s =>
            {
                var monthWorkInfo = new MonthWorkInfo { MonthName = Formatters.GetMonthAndYearNames(s.Year, s.Month), TotalSalary = s.TotalSalary };
                return s.UsersWorksDurationsInfos.Select(di => new StatisticWrapper
                {
                    MonthWorkInfo = monthWorkInfo,
                    User = di.User,
                    WorkDuration = di.WorkDuration,
                    TotalSalary = di.TotalSalary,
                });
            }).ToList();
        }

        private void FormatChartStatistic()
        {
            showDonutChart = selectedUser == null;
            options.DisableLegend = !showDonutChart;
            series = new();
            xAxisLabels = usersWorksDurationsReports.Select(r => $"{r.Month}/{r.Year}").Reverse().ToArray();

            var groupedReports = usersWorksDurationsReports
                .SelectMany(report => report.UsersWorksDurationsInfos)
                .Reverse()
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
            public MonthWorkInfo MonthWorkInfo { get; set; }
        }

        private sealed class MonthWorkInfo
        {
            public string MonthName { get; set; }
            public decimal TotalSalary { get; set; }
        }
    }
}
