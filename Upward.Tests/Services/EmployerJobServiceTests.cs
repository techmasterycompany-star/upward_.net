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
    public class EmployerJobServiceTests
    {
        private Mock<IEmployerJobRepository> _jobRepoMock = null!;
        private Mock<IEmployerRepository> _employerRepoMock = null!;
        private EmployerJobService _service = null!;

        [SetUp]
        public void Setup()
        {
            _jobRepoMock = new Mock<IEmployerJobRepository>();
            _employerRepoMock = new Mock<IEmployerRepository>();
            _service = new EmployerJobService(_jobRepoMock.Object, _employerRepoMock.Object);
        }

        [Test]
        public async Task GetJobsByEmployerAsync_ReturnsListOfJobs()
        {
            var jobs = new List<Job> { CreateSampleJob() };
            _jobRepoMock.Setup(r => r.GetByEmployerIdAsync(1)).ReturnsAsync(jobs);

            var result = await _service.GetJobsByEmployerAsync(1);

            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Software Engineer");
        }

        [Test]
        public async Task GetByIdAsync_OwnJob_ReturnsJobDetail()
        {
            var job = CreateSampleJob();
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _service.GetByIdAsync(1, 1);

            result.Should().NotBeNull();
            result!.Title.Should().Be("Software Engineer");
        }

        [Test]
        public async Task GetByIdAsync_OtherEmployerJob_ReturnsNull()
        {
            var job = CreateSampleJob(employerId: 1);
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _service.GetByIdAsync(1, 999);

            result.Should().BeNull();
        }

        [Test]
        public async Task CreateAsync_ValidRequest_CreatesJob()
        {
            var employer = CreateSampleEmployer();
            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);
            _jobRepoMock.Setup(r => r.CreateAsync(It.IsAny<Job>()))
                .ReturnsAsync((Job j) => { j.Id = 1; return j; });
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateSampleJob());

            var request = new CreateJobRequest
            {
                Title = "Backend Developer",
                Description = "Build APIs",
                Responsibilities = "Design and implement",
                Requirements = ".NET, SQL",
                WorkType = "Remote",
                ExperienceLevel = "Senior",
                ApplicationDeadline = DateTime.UtcNow.AddDays(30),
                CategoryId = 1,
                SalaryMin = 50000,
                SalaryMax = 80000
            };

            var result = await _service.CreateAsync(1, request);

            result.Should().NotBeNull();
            result.Title.Should().Be("Software Engineer"); // Returns from GetByIdAsync mock
        }

        [Test]
        public void CreateAsync_NonExistingEmployer_ThrowsException()
        {
            _employerRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EmployerProfile?)null);

            var request = new CreateJobRequest
            {
                Title = "Job",
                Description = "Desc",
                Responsibilities = "Resp",
                Requirements = "Req",
                WorkType = "Remote",
                ExperienceLevel = "Junior",
                ApplicationDeadline = DateTime.UtcNow.AddDays(30),
                CategoryId = 1
            };

            var act = () => _service.CreateAsync(999, request);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*not found*");
        }

        [Test]
        public async Task UpdateAsync_OwnJob_UpdatesFields()
        {
            var job = CreateSampleJob();
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var request = new UpdateJobRequest { Title = "Updated Title" };

            var result = await _service.UpdateAsync(1, 1, request);

            result.Should().NotBeNull();
        }

        [Test]
        public void UpdateAsync_OtherEmployerJob_ThrowsException()
        {
            var job = CreateSampleJob(employerId: 1);
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var request = new UpdateJobRequest { Title = "Hacked" };

            var act = () => _service.UpdateAsync(1, 999, request);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*permission*");
        }

        [Test]
        public async Task DeleteAsync_OwnJob_ReturnsTrue()
        {
            var job = CreateSampleJob();
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _service.DeleteAsync(1, 1);

            result.Should().BeTrue();
            job.IsDeleted.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_OtherEmployerJob_ReturnsFalse()
        {
            var job = CreateSampleJob(employerId: 1);
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _service.DeleteAsync(1, 999);

            result.Should().BeFalse();
        }

        [Test]
        public async Task CloseAsync_OwnJob_SetsStatusToClosed()
        {
            var job = CreateSampleJob();
            job.Status = JobStatus.Approved;
            _jobRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _service.CloseAsync(1, 1);

            result.Should().BeTrue();
            job.Status.Should().Be(JobStatus.Closed);
        }

        private static Job CreateSampleJob(long id = 1, long employerId = 1) => new()
        {
            Id = id,
            EmployerId = employerId,
            Title = "Software Engineer",
            Description = "Build stuff",
            Responsibilities = "Code",
            Requirements = "C#",
            WorkType = WorkType.Remote,
            ExperienceLevel = ExperienceLevel.MidLevel,
            ApplicationDeadline = DateTime.UtcNow.AddDays(30),
            Status = JobStatus.Approved,
            Category = new Category { Id = 1, Name = "Engineering" },
            Employer = new EmployerProfile
            {
                Id = employerId,
                CompanyName = "Tech Corp",
                User = new User { Name = "John", Email = "john@test.com" }
            },
            JobTechnologies = new List<JobTechnology>(),
            CreatedAt = DateTime.UtcNow
        };

        private static EmployerProfile CreateSampleEmployer() => new()
        {
            Id = 1,
            UserId = 1,
            CompanyName = "Tech Corp",
            User = new User { Name = "John", Email = "john@test.com" }
        };
    }
}
