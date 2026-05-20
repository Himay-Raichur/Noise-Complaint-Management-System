using NoiseComplaint.Models;

namespace NoiseComplaint.Repositories;

public interface IComplaintRepository
{
    Task<List<Complaint>> GetAllAsync();

    Task<Complaint?> GetByIdAsync(int id);

    Task AddAsync(Complaint complaint);

    Task UpdateAsync(Complaint complaint);

    Task DeleteAsync(Complaint complaint);

    Task SaveChangesAsync();
}