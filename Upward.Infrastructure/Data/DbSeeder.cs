using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Domain.Entities;
using Upward.Domain.Enums;
using JobApplication = Upward.Domain.Entities.Application;

namespace Upward.Infrastructure.Data
{

    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDBContext context)
        {
            // Prevent duplicate seeding
            if (context.Users.Any())
                return;

            // ============================================================
            // USERS
            // ============================================================

            var admin = new User
            {
                Name = "System Admin",
                Email = "admin@jobboard.com",
                Role = UserRole.Admin,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var employerUser = new User
            {
                Name = "Ahmed Hassan",
                Email = "ahmed@techcorp.com",
                Role = UserRole.Employer,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var employerUser2 = new User
            {
                Name = "Sara Mohamed",
                Email = "sara@innovate.com",
                Role = UserRole.Employer,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var candidateUser = new User
            {
                Name = "Omar Ali",
                Email = "omar@gmail.com",
                Role = UserRole.Candidate,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var candidateUser2 = new User
            {
                Name = "Mariam Ahmed",
                Email = "mariam@gmail.com",
                Role = UserRole.Candidate,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            employerUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employer@123");
            employerUser2.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employer@123");
            candidateUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Candidate@123");
            candidateUser2.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Candidate@123");

            context.Users.AddRange(
                admin,
                employerUser,
                employerUser2,
                candidateUser,
                candidateUser2
            );

            await context.SaveChangesAsync();


            // ============================================================
            // EMPLOYER PROFILES
            // ============================================================

            var employerProfile = new EmployerProfile
            {
                UserId = employerUser.Id,
                CompanyName = "TechCorp",
                Description = "Software development and technology company.",
                Industry = "Software Development",
                Website = "https://techcorp.com",
                CompanyLogo = "https://example.com/logos/techcorp.png",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var employerProfile2 = new EmployerProfile
            {
                UserId = employerUser2.Id,
                CompanyName = "Innovate Solutions",
                Description = "Technology company building modern digital products.",
                Industry = "Technology",
                Website = "https://innovate.com",
                CompanyLogo = "https://example.com/logos/innovate.png",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.EmployerProfiles.AddRange(
                employerProfile,
                employerProfile2
            );

            await context.SaveChangesAsync();


            // ============================================================
            // CANDIDATE PROFILES
            // ============================================================

            var candidateProfile = new CandidateProfile
            {
                UserId = candidateUser.Id,
                Headline = ".NET Backend Developer",
                Bio = "Backend developer interested in scalable web applications.",
                Location = "Alexandria, Egypt",
                PortfolioUrl = "https://portfolio.example.com/omar",
                ResumeUrl = "resumes/omar-resume.pdf",
                LinkedinProfile = "https://linkedin.com/in/omar",
                IsDiscoverable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var candidateProfile2 = new CandidateProfile
            {
                UserId = candidateUser2.Id,
                Headline = "Frontend Angular Developer",
                Bio = "Frontend developer specializing in Angular and TypeScript.",
                Location = "Cairo, Egypt",
                PortfolioUrl = "https://portfolio.example.com/mariam",
                ResumeUrl = "resumes/mariam-resume.pdf",
                LinkedinProfile = "https://linkedin.com/in/mariam",
                IsDiscoverable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.CandidateProfiles.AddRange(
                candidateProfile,
                candidateProfile2
            );

            await context.SaveChangesAsync();


            // ============================================================
            // CATEGORIES
            // ============================================================

            var categories = new[]
            {
            new Category
            {
                Name = "Software Development",
                Description = "Software engineering and application development.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Data Science",
                Description = "Data science, analytics and machine learning.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "DevOps",
                Description = "Infrastructure, cloud and DevOps engineering.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "UI/UX Design",
                Description = "User interface and user experience design.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();


            // ============================================================
            // TECHNOLOGIES
            // ============================================================

            var technologies = new[]
            {
            new Technology
            {
                Name = ".NET",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "ASP.NET Core",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "Angular",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "React",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "SQL Server",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "Docker",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Technology
            {
                Name = "Python",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.Technologies.AddRange(technologies);
            await context.SaveChangesAsync();


            // ============================================================
            // SKILLS
            // ============================================================

            var skills = new[]
            {
            new Skill
            {
                Name = "C#",
                Category = "Programming Languages",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Skill
            {
                Name = "ASP.NET Core",
                Category = "Backend",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Skill
            {
                Name = "SQL",
                Category = "Database",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Skill
            {
                Name = "Angular",
                Category = "Frontend",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Skill
            {
                Name = "TypeScript",
                Category = "Programming Languages",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Skill
            {
                Name = "Docker",
                Category = "DevOps",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.Skills.AddRange(skills);
            await context.SaveChangesAsync();


            // ============================================================
            // CANDIDATE SKILLS
            // ============================================================

            var candidateSkills = new[]
            {
            new CandidateSkill
            {
                CandidateProfileId = candidateProfile.Id,
                SkillId = 1,
                YearsExperience = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CandidateSkill
            {
                CandidateProfileId = candidateProfile.Id,
                SkillId = 2,
                YearsExperience = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CandidateSkill
            {
                CandidateProfileId = candidateProfile.Id,
                SkillId = 3,
                YearsExperience = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CandidateSkill
            {
                CandidateProfileId = candidateProfile2.Id,
                SkillId = 4,
                YearsExperience = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CandidateSkill
            {
                CandidateProfileId = candidateProfile2.Id,
                SkillId = 5,
                YearsExperience = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.CandidateSkills.AddRange(candidateSkills);
            await context.SaveChangesAsync();


            // ============================================================
            // PLANS
            // ============================================================

            var freePlan = new Plan
            {
                Name = "Free",
                JobPostLimit = 3,
                PriceMonthly = 0,
                PriceYearly = 0,
                IsFeatured = false,
                HasDirectMessaging = false,
                HasPremiumReports = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var proPlan = new Plan
            {
                Name = "Pro",
                JobPostLimit = 20,
                PriceMonthly = 29.99m,
                PriceYearly = 299.99m,
                IsFeatured = true,
                HasDirectMessaging = true,
                HasPremiumReports = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var enterprisePlan = new Plan
            {
                Name = "Enterprise",
                JobPostLimit = null, // Unlimited
                PriceMonthly = 99.99m,
                PriceYearly = 999.99m,
                IsFeatured = true,
                HasDirectMessaging = true,
                HasPremiumReports = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Plans.AddRange(
                freePlan,
                proPlan,
                enterprisePlan
            );

            await context.SaveChangesAsync();


            // ============================================================
            // SUBSCRIPTIONS
            // ============================================================

            var subscription = new Subscription
            {
                EmployerId = employerProfile.Id,
                PlanId = freePlan.Id,
                BillingCycle = BillingCycle.Monthly,
                Status = SubscriptionStatus.Active,
                CurrentPeriodStart = DateTime.UtcNow.Date,
                CurrentPeriodEnd = DateTime.UtcNow.Date.AddMonths(1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var subscription2 = new Subscription
            {
                EmployerId = employerProfile2.Id,
                PlanId = proPlan.Id,
                BillingCycle = BillingCycle.Monthly,
                Status = SubscriptionStatus.Active,
                CurrentPeriodStart = DateTime.UtcNow.Date,
                CurrentPeriodEnd = DateTime.UtcNow.Date.AddMonths(1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Subscriptions.AddRange(
                subscription,
                subscription2
            );

            await context.SaveChangesAsync();


            // ============================================================
            // JOBS
            // ============================================================

            var job1 = new Job
            {
                EmployerId = employerProfile.Id,
                CategoryId = 1,
                Title = ".NET Backend Developer",
                Description = "We are looking for a .NET Backend Developer.",
                Responsibilities = "Develop APIs and backend services.",
                Requirements = "Strong C# and ASP.NET Core knowledge.",
                Location = "Cairo, Egypt",
                WorkType = WorkType.Hybrid,
                SalaryMin = 15000,
                SalaryMax = 25000,
                ExperienceLevel = ExperienceLevel.Junior,
                ApplicationDeadline = DateTime.UtcNow.AddDays(30),
                Status = JobStatus.Approved,
                ViewsCount = 25,
                ApplicationsCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var job2 = new Job
            {
                EmployerId = employerProfile2.Id,
                CategoryId = 1,
                Title = "Angular Frontend Developer",
                Description = "Build modern web applications using Angular.",
                Responsibilities = "Develop reusable Angular components.",
                Requirements = "Angular, TypeScript and RxJS experience.",
                Location = "Cairo, Egypt",
                WorkType = WorkType.Remote,
                SalaryMin = 12000,
                SalaryMax = 20000,
                ExperienceLevel = ExperienceLevel.MidLevel,
                ApplicationDeadline = DateTime.UtcNow.AddDays(45),
                Status = JobStatus.Approved,
                ViewsCount = 40,
                ApplicationsCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var job3 = new Job
            {
                EmployerId = employerProfile.Id,
                CategoryId = 3,
                Title = "DevOps Engineer",
                Description = "Manage cloud infrastructure and deployment pipelines.",
                Responsibilities = "Maintain CI/CD pipelines and infrastructure.",
                Requirements = "Docker, cloud and CI/CD experience.",
                Location = "Alexandria, Egypt",
                WorkType = WorkType.OnSite,
                SalaryMin = 18000,
                SalaryMax = 30000,
                ExperienceLevel = ExperienceLevel.Senior,
                ApplicationDeadline = DateTime.UtcNow.AddDays(20),
                Status = JobStatus.PendingApproval,
                ViewsCount = 0,
                ApplicationsCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Jobs.AddRange(job1, job2, job3);
            await context.SaveChangesAsync();


            // ============================================================
            // JOB TECHNOLOGIES
            // ============================================================

            var jobTechnologies = new[]
            {
            new JobTechnology
            {
                JobId = job1.Id,
                TechnologyId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new JobTechnology
            {
                JobId = job1.Id,
                TechnologyId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new JobTechnology
            {
                JobId = job1.Id,
                TechnologyId = 5,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new JobTechnology
            {
                JobId = job2.Id,
                TechnologyId = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new JobTechnology
            {
                JobId = job2.Id,
                TechnologyId = 4,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new JobTechnology
            {
                JobId = job3.Id,
                TechnologyId = 6,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.JobTechnologies.AddRange(jobTechnologies);
            await context.SaveChangesAsync();


            // ============================================================
            // APPLICATIONS
            // ============================================================

            var application = new JobApplication
            {
                JobId = job1.Id,
                CandidateId = candidateProfile.Id,
                Resume = "resumes/omar-resume.pdf",
                CoverLetter = "I am excited to apply for this position.",
                Message = "I would love to join your team.",
                ContactEmail = candidateUser.Email,
                ContactPhone = "+201000000000",
                Status = ApplicationStatus.UnderReview,
                ReviewedAt = null,
                RejectionReason = null,
                AppliedViaLinkedIn = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var application2 = new JobApplication
            {
                JobId = job2.Id,
                CandidateId = candidateProfile2.Id,
                Resume = "resumes/mariam-resume.pdf",
                CoverLetter = "I am interested in the Angular Developer position.",
                Message = null,
                ContactEmail = candidateUser2.Email,
                ContactPhone = "+201100000000",
                Status = ApplicationStatus.Submitted,
                AppliedViaLinkedIn = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Applications.AddRange(
                application,
                application2
            );

            await context.SaveChangesAsync();


            // ============================================================
            // COMMENTS
            // ============================================================

            var comments = new[]
            {
            new Comment
            {
                JobId = job1.Id,
                UserId = candidateUser.Id,
                Content = "Is this position open to fresh graduates?",
                IsApproved = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Comment
            {
                JobId = job1.Id,
                UserId = employerUser.Id,
                Content = "Yes, candidates with strong .NET fundamentals are welcome.",
                IsApproved = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.Comments.AddRange(comments);
            await context.SaveChangesAsync();


            // ============================================================
            // WISHLIST
            // ============================================================

            var wishlist = new Wishlist
            {
                CandidateId = candidateProfile.Id,
                JobId = job2.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Wishlists.Add(wishlist);

            await context.SaveChangesAsync();


            // ============================================================
            // NOTIFICATIONS
            // ============================================================

            var notifications = new[]
            {
            new Notification
            {
                UserId = candidateUser.Id,
                Type = NotificationType.ApplicationStatusChanged,
                Title = "Application Under Review",
                Content = "Your application is currently under review.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Notification
            {
                UserId = employerUser.Id,
                Type = NotificationType.ApplicationSubmitted,
                Title = "New Application",
                Content = "A candidate has applied to your job.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

            context.Notifications.AddRange(notifications);

            await context.SaveChangesAsync();
        }
    }
}
