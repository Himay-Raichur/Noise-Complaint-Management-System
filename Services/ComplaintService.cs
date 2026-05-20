using NoiseComplaint.Models;
using NoiseComplaint.Repositories;

namespace NoiseComplaint.Services;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _repository;

    public ComplaintService(IComplaintRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Complaint>> GetAllComplaintsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Complaint?> GetComplaintByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateComplaintAsync(Complaint complaint)
    {
        await _repository.AddAsync(complaint);

        await _repository.SaveChangesAsync();
    }
}