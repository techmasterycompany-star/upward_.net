using System.ComponentModel.DataAnnotations;


namespace Upward.Application.DTOs.Admin
{
    public class RejectJobRequest
    {
        [Required(ErrorMessage = "Rejection reason is required.")]
        public string Reason { get; set; } = null!;
    }
}
