namespace NoiseComplaint.DTOs.Complaints;

public class ComplaintQueryParametersDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 5;

    public string? Status { get; set; }

    public string? Search { get; set; }

    public string SortBy { get; set; } = "createdAt";

    public bool Descending { get; set; } = true;
}