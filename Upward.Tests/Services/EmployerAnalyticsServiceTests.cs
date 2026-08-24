using FluentAssertions;
using Moq;
using NUnit.Framework;
using Upward.Application.DTOs.Employer;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Services;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Tests.Services
{
    [TestFixture]
    public class EmployerAnalyticsServiceTests
    {
        private Mock<IEmployerAnalyticsRepository> _analyticsRepoMock = null!;
        private EmployerAnalyticsService _service = null!;

        [SetUp]
        public void Setup()
        {
            _analyticsRepoMock = new Mock<IEmployerAnalyticsRepository>();
            _service = new EmployerAnalyticsService(_analyticsRepoMock.Object);
        }

        [Test]
        public async Task GetDashboardAsync_ReturnsCorrectStats()
        {
            var jobs = new List<Job>
            {
                CreateJobWithStats(1, "Job 1", 100, 5, 2, 1, 2, JobStatus.Approved),
                CreateJobWithStats(2, "Job 2", 50, 3, 1, 0, 2, JobStatus.Approved),
                CreateJobWithStats(3, "Job 3", 0, 0, 0, 0, 0, JobStatus.Closed)
            };
            _analyticsRepoMock.Setup(r => r.GetJobsWithStatsAsync(1)).ReturnsAsync(jobs);

            var result = await _service.GetDashboardAsync(1);

            result.Should().NotBeNull();
            result.TotalJobs.Should().Be(3);
            result.ActiveJobs.Should().Be(2);
            result.TotalApplications.Should().Be(8);
            result.TotalViews.Should().Be(150);
            result.TopJobs.Should().HaveCount(3);
        }

        [Test]
        public async Task GetJobAnalyticsAsync_ReturnsAnalyticsForAllJobs()
        {
            var jobs = new List<Job>
            {
                CreateJobWithStats(1, "Job 1", 100, 5, 2, 1, 2, JobStatus.Approved),
                CreateJobWithStats(2, "Job 2", 50, 3, 1, 0, 2, JobStatus.Approved)
            };
            _analyticsRepoMock.Setup(r => r.GetJobsWithStatsAsync(1)).ReturnsAsync(jobs);

            var result = await _service.GetJobAnalyticsAsync(1);

            result.Should().HaveCount(2);
            result[0].JobTitle.Should().Be("Job 1");
            result[0].ViewsCount.Should().Be(100);
            result[0].ApplicationsCount.Should().Be(5);
        }

        [Test]
        public async Task SearchCandidatesAsync_WithKeyword_ReturnsMatchingCandidates()
        {
            var candidates = new List<CandidateProfile>
            {
                new()
                {
                    Id = 1,
                    User = new User { Name = "John Doe", Email = "john@test.com" },
                    Headline = "Full Stack Developer",
                    Location = "Cairo",
                    Resume = "https://resume.com/john",
                    IsDiscoverable = true,
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new() { Skill = new Skill { Name = "C#" } }
                    }
                }
            };
            _analyticsRepoMock.Setup(r => r.SearchCandidatesAsync("John", null)).ReturnsAsync(candidates);

            var result = await _service.SearchCandidatesAsync("John", null);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("John Doe");
        }

        [Test]
        public async Task SearchCandidatesAsync_WithSkills_ReturnsMatchingCandidates()
        {
            var candidates = new List<CandidateProfile>
            {
                new()
                {
                    Id = 1,
                    User = new User { Name = "Jane", Email = "jane@test.com" },
                    Headline = "Backend Dev",
                    IsDiscoverable = true,
                    CandidateSkills = new List<CandidateSkill>
                    {
                        new() { Skill = new Skill { Name = "C#" } },
                        new() { Skill = new Skill { Name = ".NET" } }
                    }
                }
            };
            _analyticsRepoMock.Setup(r => r.SearchCandidatesAsync(null, new List<string> { "C#" })).ReturnsAsync(candidates);

            var result = await _service.SearchCandidatesAsync(null, new List<string> { "C#" });

            result.Should().HaveCount(1);
            result[0].Skills.Should().Contain("C#");
        }

        [Test]
        public async Task SearchCandidatesAsync_NoFilters_ReturnsAllDiscoverable()
        {
            var candidates = new List<CandidateProfile>
            {
                new()
                {
                    Id = 1,
                    User = new User { Name = "A", Email = "a@test.com" },
                    IsDiscoverable = true,
                    CandidateSkills = new List<CandidateSkill>()
                },
                new()
                {
                    Id = 2,
                    User = new User { Name = "B", Email = "b@test.com" },
                    IsDiscoverable = true,
                    CandidateSkills = new List<CandidateSkill>()
                }
            };
            _analyticsRepoMock.Setup(r => r.SearchCandidatesAsync(null, null)).ReturnsAsync(candidates);

            var result = await _service.SearchCandidatesAsync(null, null);

            result.Should().HaveCount(2);
        }

        private static Job CreateJobWithStats(long id, string title, int views, int totalApps,
            int accepted, int rejected, int pending, JobStatus status)
        {
            var applications = new List<Upward.Domain.Entities.Application>();
            for (int i = 0; i < accepted; i++)
                applications.Add(new Upward.Domain.Entities.Application { Status = ApplicationStatus.Accepted });
            for (int i = 0; i < rejected; i++)
                applications.Add(new Upward.Domain.Entities.Application { Status = ApplicationStatus.Rejected });
            for (int i = 0; i < pending; i++)
                applications.Add(new Upward.Domain.Entities.Application { Status = ApplicationStatus.Submitted });

            return new Job
            {
                Id = id,
                EmployerId = 1,
                Title = title,
                ViewsCount = views,
                Status = status,
                Applications = applications,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
