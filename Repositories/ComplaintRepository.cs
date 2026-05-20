using Microsoft.EntityFrameworkCore;
using NoiseComplaint.Data;
using NoiseComplaint.Models;

namespace NoiseComplaint.Repositories;

public class ComplaintRepository : IComplaintRepository
{
    private readonly AppDbContext _context;

    public ComplaintRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Complaint>> GetAllAsync()
    {
        return await _context.Complaints.ToListAsync();
    }

    public async Task<Complaint?> GetByIdAsync(int id)
    {
        return await _context.Complaints
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Complaint complaint)
    {
        await _context.Complaints.AddAsync(complaint);
    }

    public async Task UpdateAsync(Complaint complaint)
    {
        _context.Complaints.Update(complaint);

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Complaint complaint)
    {
        _context.Complaints.Remove(complaint);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}