namespace NoiseComplaint.DTOs.Dashboard;

public class ComplaintStatsDto
{
    public int TotalComplaints { get; set; }

    public int PendingComplaints { get; set; }

    public int InProgressComplaints { get; set; }

    public int ResolvedComplaints { get; set; }

    public int RejectedComplaints { get; set; }
}