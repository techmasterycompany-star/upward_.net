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
    public class EmployerServiceTests
    {
        private Mock<IEmployerRepository> _employerRepoMock = null!;
        private EmployerService _service = null!;

        [SetUp]
        public void Setup()
        {
            _employerRepoMock = new Mock<IEmployerRepository>();
            _service = new EmployerService(_employerRepoMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ExistingEmployer_ReturnsDto()
        {
            var employer = CreateSampleEmployer();
            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.CompanyName.Should().Be("Tech Corp");
            result.UserName.Should().Be("John Doe");
        }

        [Test]
        public async Task GetByIdAsync_NonExistingEmployer_ReturnsNull()
        {
            _employerRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EmployerProfile?)null);

            var result = await _service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Test]
        public async Task GetByUserIdAsync_ExistingEmployer_ReturnsDto()
        {
            var employer = CreateSampleEmployer();
            _employerRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(employer);

            var result = await _service.GetByUserIdAsync(1);

            result.Should().NotBeNull();
            result!.UserId.Should().Be(1);
        }

        [Test]
        public async Task GetAllAsync_ReturnsListOfEmployers()
        {
            var employers = new List<EmployerProfile> { CreateSampleEmployer(), CreateSampleEmployer(2) };
            _employerRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employers);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Test]
        public async Task SearchAsync_WithKeyword_ReturnsFilteredList()
        {
            var employers = new List<EmployerProfile> { CreateSampleEmployer() };
            _employerRepoMock.Setup(r => r.SearchAsync("Tech")).ReturnsAsync(employers);

            var result = await _service.SearchAsync("Tech");

            result.Should().HaveCount(1);
            result[0].CompanyName.Should().Be("Tech Corp");
        }

        [Test]
        public async Task CreateAsync_ValidRequest_CreatesEmployer()
        {
            _employerRepoMock.Setup(r => r.ExistsByUserIdAsync(1)).ReturnsAsync(false);
            _employerRepoMock.Setup(r => r.CreateAsync(It.IsAny<EmployerProfile>()))
                .ReturnsAsync((EmployerProfile e) => { e.Id = 1; return e; });

            var request = new CreateEmployerProfileRequest
            {
                UserId = 1,
                CompanyName = "New Corp",
                Description = "A new company"
            };

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.CompanyName.Should().Be("New Corp");
            result.UserId.Should().Be(1);
        }

        [Test]
        public void CreateAsync_DuplicateUserId_ThrowsException()
        {
            _employerRepoMock.Setup(r => r.ExistsByUserIdAsync(1)).ReturnsAsync(true);

            var request = new CreateEmployerProfileRequest
            {
                UserId = 1,
                CompanyName = "Duplicate Corp"
            };

            var act = () => _service.CreateAsync(request);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*already has an employer profile*");
        }

        [Test]
        public async Task UpdateAsync_ExistingEmployer_UpdatesFields()
        {
            var employer = CreateSampleEmployer();
            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);
            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);

            var request = new UpdateJobRequest { Title = "Updated Title" };

            // Test that update doesn't throw
            var updateRequest = new UpdateEmployerProfileRequest { CompanyName = "Updated Corp" };
            var result = await _service.UpdateAsync(1, updateRequest);

            result.Should().NotBeNull();
            result.CompanyName.Should().Be("Updated Corp");
        }

        [Test]
        public void UpdateAsync_NonExistingEmployer_ThrowsException()
        {
            _employerRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EmployerProfile?)null);

            var request = new UpdateEmployerProfileRequest { CompanyName = "Updated" };

            var act = () => _service.UpdateAsync(999, request);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*not found*");
        }

        [Test]
        public async Task DeleteAsync_ExistingEmployer_ReturnsTrue()
        {
            var employer = CreateSampleEmployer();
            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_NonExistingEmployer_ReturnsFalse()
        {
            _employerRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EmployerProfile?)null);

            var result = await _service.DeleteAsync(999);

            result.Should().BeFalse();
        }

        [Test]
        public async Task GetJobsAsync_ExistingEmployer_ReturnsJobs()
        {
            var employer = CreateSampleEmployer();
            employer.Jobs.Add(new Job
            {
                Id = 1,
                Title = "Software Engineer",
                Description = "Build stuff",
                Responsibilities = "Code",
                Requirements = "C#",
                WorkType = WorkType.Remote,
                ExperienceLevel = ExperienceLevel.MidLevel,
                ApplicationDeadline = DateTime.UtcNow.AddDays(30),
                Status = JobStatus.Approved,
                CreatedAt = DateTime.UtcNow
            });

            _employerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employer);

            var result = await _service.GetJobsAsync(1);

            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Software Engineer");
        }

        private static EmployerProfile CreateSampleEmployer(long id = 1) => new()
        {
            Id = id,
            UserId = id,
            CompanyName = "Tech Corp",
            Description = "A tech company",
            Industry = "Technology",
            Website = "https://techcorp.com",
            User = new User { Id = id, Name = "John Doe", Email = "john@techcorp.com" },
            Jobs = new List<Job>(),
            Subscriptions = new List<Subscription>()
        };
    }
}
