using Microsoft.EntityFrameworkCore;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data.Configurations;

namespace Upward.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();
        public DbSet<EmployerProfile> EmployerProfiles => Set<EmployerProfile>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<JobApplication> Applications => Set<JobApplication>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
        public DbSet<Technology> Technologies => Set<Technology>();
        public DbSet<JobTechnology> JobTechnologies => Set<JobTechnology>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<CommentReport> CommentReports => Set<CommentReport>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();
        public DbSet<SavedSearch> SavedSearches => Set<SavedSearch>();
        public DbSet<JobView> JobViews => Set<JobView>();
        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
    }
}
