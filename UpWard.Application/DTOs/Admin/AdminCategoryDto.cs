namespace Upwork.Application.DTOs.Admin
{
    public class AdminCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int JobsCount { get; set; }
    }
}
