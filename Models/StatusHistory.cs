namespace NoiseComplaint.Models
{
    public class StatusHistory
    {
        public int Id { get; set; }

        public int ComplaintId { get; set; }

        public Complaint Complaint { get; set; } = null!;

        public string OldStatus { get; set; } = string.Empty;

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
            = DateTime.UtcNow;

        public int ChangedByUserId { get; set; }

        public User ChangedByUser { get; set; } = null!;
    }
}