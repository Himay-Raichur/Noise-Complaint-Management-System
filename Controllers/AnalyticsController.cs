using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiseComplaint.Data;
using NoiseComplaint.DTOs.Dashboard;

namespace NoiseComplaint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("complaint-stats")]
    public async Task<ActionResult<ComplaintStatsDto>> GetComplaintStats()
    {
        var stats = new ComplaintStatsDto
        {
            TotalComplaints = await _context.Complaints.CountAsync(),

            PendingComplaints = await _context.Complaints
                .CountAsync(c => c.Status == "Pending"),

            InProgressComplaints = await _context.Complaints
                .CountAsync(c => c.Status == "InProgress"),

            ResolvedComplaints = await _context.Complaints
                .CountAsync(c => c.Status == "Resolved"),

            RejectedComplaints = await _context.Complaints
                .CountAsync(c => c.Status == "Rejected")
        };

        return Ok(stats);
    }

    [HttpGet("status-breakdown")]
public async Task<ActionResult<IEnumerable<StatusBreakdownDto>>> GetStatusBreakdown()
{
    var result = await _context.Complaints
        .GroupBy(c => c.Status)
        .Select(group => new StatusBreakdownDto
        {
            Status = group.Key,
            Count = group.Count()
        })
        .ToListAsync();

    return Ok(result);
}
}