using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Moq;
using NUnit.Framework;
using QuickActions.Common.Specifications;
using WorkTimer.Api.Repository;
using WorkTimer.Api.Services;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Tests.Services;

[TestFixture]
[TestOf(typeof(WorkPeriodsService))]
public class WorkPeriodsServiceTest
{
    private Mock<WorkPeriodRepository> _workPeriodRepositoryMock;
    private Faker<WorkPeriod> _workPeriodFaker;

    [SetUp]
    public void Setup()
    {
        _workPeriodFaker = new Faker<WorkPeriod>()
            .RuleFor(wp => wp.StartAt, f => f.Date.Recent())
            .RuleFor(wp => wp.EndAt, f => f.Date.Future())
            .RuleFor(wp => wp.UserId, f => f.Random.Int(1, 100));

        var dbContextMock = new Mock<AppDbContext>();
        _workPeriodRepositoryMock = new Mock<WorkPeriodRepository>(dbContextMock.Object);

        var fakeWorkPeriods = new List<WorkPeriod>();
        for (int i = 1; i <= 30; i++)
        {
            var startAt = new DateTime(2024, 4, i, 8, 0, 0);
            var endAt = startAt.AddHours(8);
            var workPeriod = new WorkPeriod
            {
                StartAt = startAt,
                EndAt = endAt,
                UserId = _workPeriodFaker.Generate().UserId
            };
            fakeWorkPeriods.Add(workPeriod);
        }

        _workPeriodRepositoryMock.Setup(repo => repo.Read(It.IsAny<Specification<WorkPeriod>>(), 0, int.MaxValue))
            .ReturnsAsync(fakeWorkPeriods);
    }

    [Test]
    public async Task GetMonthStatistic_ReturnsCorrectResult()
    {
        // Arrange
        var currentUser = new User { Id = 1 };
        var monthDateTime = new DateTime(2024, 4, 1);

        var monthStatisticCalculator = new WorkPeriodsService(_workPeriodRepositoryMock.Object);

        // Act
        var result = await monthStatisticCalculator.GetMonthStatistic(monthDateTime, currentUser);

        // Assert
        Assert.That(result.Count, Is.EqualTo(30));
        Assert.That(result.Values.All(percent => percent >= 0 && percent <= 100), Is.True);
    }

    [Test]
    public async Task GetMonthStatistic_NoWorkPeriods_ReturnsEmptyDictionary()
    {
        // Arrange
        var currentUser = new User { Id = 1 };
        var monthDateTime = new DateTime(2024, 4, 1);
        _workPeriodRepositoryMock.Setup(repo => repo.Read(It.IsAny<Specification<WorkPeriod>>(), 0, int.MaxValue))
            .ReturnsAsync(new List<WorkPeriod>());

        var monthStatisticCalculator = new WorkPeriodsService(_workPeriodRepositoryMock.Object);

        // Act
        var result = await monthStatisticCalculator.GetMonthStatistic(monthDateTime, currentUser);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetMonthStatistic_WorkPeriodsForMultipleUsers_ReturnsOnlyCurrentUserStatistics()
    {
        // Arrange
        var currentUser = new User { Id = 1 };
        var monthDateTime = new DateTime(2024, 4, 1);
        var fakeWorkPeriods = _workPeriodFaker
            .Generate(20)
            .Select((wp, index) =>
            {
                wp.UserId = index % 2 == 0 ? 1 : 2;
                return wp;
            });
        _workPeriodRepositoryMock.Setup(repo => repo.Read(It.IsAny<Specification<WorkPeriod>>(), 0, int.MaxValue))
            .ReturnsAsync(fakeWorkPeriods.ToList());

        var monthStatisticCalculator = new WorkPeriodsService(_workPeriodRepositoryMock.Object);

        // Act
        var result = await monthStatisticCalculator.GetMonthStatistic(monthDateTime, currentUser);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
    }
}