using System.ComponentModel.DataAnnotations;

namespace NoiseComplaint.DTOs;

public class CreateComplaintDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;
}