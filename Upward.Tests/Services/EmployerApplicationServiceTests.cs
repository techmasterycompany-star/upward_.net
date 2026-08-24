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
    public class EmployerApplicationServiceTests
    {
        private Mock<IEmployerApplicationRepository> _appRepoMock = null!;
        private EmployerApplicationService _service = null!;

        [SetUp]
        public void Setup()
        {
            _appRepoMock = new Mock<IEmployerApplicationRepository>();
            _service = new EmployerApplicationService(_appRepoMock.Object);
        }

        [Test]
        public async Task GetApplicationsByEmployerAsync_ReturnsListOfApplications()
        {
            var applications = new List<Upward.Domain.Entities.Application> { CreateSampleApplication() };
            _appRepoMock.Setup(r => r.GetByEmployerIdAsync(1)).ReturnsAsync(applications);

            var result = await _service.GetApplicationsByEmployerAsync(1);

            result.Should().HaveCount(1);
            result[0].CandidateName.Should().Be("Jane Smith");
        }

        [Test]
        public async Task GetByIdAsync_ExistingApplication_ReturnsDto()
        {
            var application = CreateSampleApplication();
            _appRepoMock.Setup(r => r.ExistsByJobEmployerAsync(1, 1)).ReturnsAsync(true);
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var result = await _service.GetByIdAsync(1, 1);

            result.Should().NotBeNull();
            result!.CandidateName.Should().Be("Jane Smith");
        }

        [Test]
        public async Task GetByIdAsync_NonExistingApplication_ReturnsNull()
        {
            _appRepoMock.Setup(r => r.ExistsByJobEmployerAsync(999, 1)).ReturnsAsync(false);

            var result = await _service.GetByIdAsync(999, 1);

            result.Should().BeNull();
        }

        [Test]
        public async Task AcceptAsync_SubmittedApplication_AcceptsSuccessfully()
        {
            var application = CreateSampleApplication();
            application.Status = ApplicationStatus.Submitted;
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var result = await _service.AcceptAsync(1, 1);

            result.Should().NotBeNull();
            result.Status.Should().Be("Accepted");
        }

        [Test]
        public async Task AcceptAsync_UnderReviewApplication_AcceptsSuccessfully()
        {
            var application = CreateSampleApplication();
            application.Status = ApplicationStatus.UnderReview;
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var result = await _service.AcceptAsync(1, 1);

            result.Status.Should().Be("Accepted");
        }

        [Test]
        public void AcceptAsync_AlreadyAccepted_ThrowsException()
        {
            var application = CreateSampleApplication();
            application.Status = ApplicationStatus.Accepted;
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var act = () => _service.AcceptAsync(1, 1);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*current status*");
        }

        [Test]
        public void AcceptAsync_OtherEmployerApplication_ThrowsException()
        {
            var application = CreateSampleApplication();
            application.Job = new Job { EmployerId = 1, Title = "Job" };
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var act = () => _service.AcceptAsync(1, 999);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*permission*");
        }

        [Test]
        public async Task RejectAsync_SubmittedApplication_RejectsWithReason()
        {
            var application = CreateSampleApplication();
            application.Status = ApplicationStatus.Submitted;
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var request = new ReviewApplicationRequest { RejectionReason = "Not a good fit" };

            var result = await _service.RejectAsync(1, 1, request);

            result.Should().NotBeNull();
            result.Status.Should().Be("Rejected");
            result.RejectionReason.Should().Be("Not a good fit");
        }

        [Test]
        public void RejectAsync_OtherEmployerApplication_ThrowsException()
        {
            var application = CreateSampleApplication();
            application.Job = new Job { EmployerId = 1, Title = "Job" };
            _appRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(application);

            var request = new ReviewApplicationRequest { RejectionReason = "Bad" };

            var act = () => _service.RejectAsync(1, 999, request);

            act.Should().ThrowAsync<Exception>()
                .WithMessage("*permission*");
        }

        private static Upward.Domain.Entities.Application CreateSampleApplication() => new()
        {
            Id = 1,
            JobId = 1,
            CandidateId = 1,
            ContactEmail = "jane@test.com",
            ContactPhone = "1234567890",
            Status = ApplicationStatus.Submitted,
            CoverLetter = "I am interested",
            Job = new Job
            {
                Id = 1,
                EmployerId = 1,
                Title = "Software Engineer"
            },
            Candidate = new CandidateProfile
            {
                Id = 1,
                UserId = 1,
                Headline = "Full Stack Developer",
                Location = "Cairo",
                Resume = "https://resume.com/jane",
                User = new User { Name = "Jane Smith", Email = "jane@test.com" },
                CandidateSkills = new List<CandidateSkill>
                {
                    new() { Skill = new Skill { Name = "C#" } },
                    new() { Skill = new Skill { Name = ".NET" } }
                }
            },
            CreatedAt = DateTime.UtcNow
        };
    }
}
