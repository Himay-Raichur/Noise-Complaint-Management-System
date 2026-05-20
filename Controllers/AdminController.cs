using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiseComplaint.Data;
using NoiseComplaint.DTOs;
using NoiseComplaint.Models;
using System.Security.Claims;

namespace NoiseComplaint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL COMPLAINTS
        [HttpGet("complaints")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _context.Complaints
                .Include(c => c.User)
                .ToListAsync();

            return Ok(complaints);
        }

        // MODERATE COMPLAINT
[HttpPut("moderate/{id}")]
public async Task<IActionResult> ModerateComplaint(
    int id,
    UpdateComplaintStatusDto dto)
{
    var complaint = await _context.Complaints
        .FirstOrDefaultAsync(c => c.Id == id);

    if (complaint == null)
    {
        return NotFound("Complaint not found");
    }

    if (!IsValidStatusTransition(
        complaint.Status,
        dto.NewStatus))
    {
        return BadRequest(
            $"Invalid transition from " +
            $"{complaint.Status} to {dto.NewStatus}");
    }

    var oldStatus = complaint.Status;

    var adminUserId = int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)!.Value
    );

    // Update complaint status
    complaint.Status = dto.NewStatus;

    // Create history record
    var history = new StatusHistory
    {
        ComplaintId = complaint.Id,
        OldStatus = oldStatus,
        NewStatus = dto.NewStatus,
        ChangedAt = DateTime.UtcNow,
        ChangedByUserId = adminUserId
    };

    // IMPORTANT
    _context.StatusHistories.Add(history);
    Console.WriteLine($"Complaint Id: {complaint.Id}");
Console.WriteLine($"Admin User Id: {adminUserId}");

    await _context.SaveChangesAsync();

return Ok(new
{
    message = "Complaint moderated successfully",
    complaintId = complaint.Id,
    oldStatus = oldStatus,
    newStatus = complaint.Status
});
}

        private bool IsValidStatusTransition(
            string currentStatus,
            string newStatus)
        {
            return currentStatus switch
            {
                ComplaintStatus.Pending =>
                    newStatus == ComplaintStatus.InProgress ||
                    newStatus == ComplaintStatus.Rejected,

                ComplaintStatus.InProgress =>
                    newStatus == ComplaintStatus.Resolved,

                ComplaintStatus.Resolved => false,

                ComplaintStatus.Rejected => false,

                _ => false
            };
        }
    }
}