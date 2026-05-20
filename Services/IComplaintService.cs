using NoiseComplaint.Models;

namespace NoiseComplaint.Services;

public interface IComplaintService
{
    Task<List<Complaint>> GetAllComplaintsAsync();

    Task<Complaint?> GetComplaintByIdAsync(int id);

    Task CreateComplaintAsync(Complaint complaint);
}