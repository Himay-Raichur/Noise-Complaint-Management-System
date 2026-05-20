using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiseComplaint.Data;
using NoiseComplaint.DTOs;
using NoiseComplaint.Models;
using System.Security.Claims;
using NoiseComplaint.DTOs.Common;
using NoiseComplaint.DTOs.Complaints;
using NoiseComplaint.Repositories;
using NoiseComplaint.Services;

namespace NoiseComplaint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly AppDbContext _context;
private readonly IComplaintRepository _repository;
private readonly IComplaintService _service;
private readonly ILogger<ComplaintsController> _logger;

       public ComplaintsController(
    AppDbContext context,
    IComplaintRepository repository,
    IComplaintService service,
    ILogger<ComplaintsController> logger)
{
    _context = context;
    _repository = repository;
    _service = service;
    _logger = logger;
}

        // CREATE COMPLAINT
        // POST: /api/complaints
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComplaint(
            CreateComplaintDto dto
        )
        {
            // Get logged-in user ID from JWT token
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

          var complaint = new Complaint
{
    Title = dto.Title,
    Description = dto.Description,
    Location = dto.Location,
    UserId = userId,
    Status = ComplaintStatus.Pending
};

            _context.Complaints.Add(complaint);

            await _context.SaveChangesAsync();

            return Ok(complaint);
        }

        // GET ALL COMPLAINTS
        // GET: /api/complaints
        [HttpGet]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _context.Complaints
                .Include(c => c.User)
                .ToListAsync();

            return Ok(complaints);
        }

        // GET COMPLAINT BY ID
        // GET: /api/complaints/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound("Complaint not found");
            }

            return Ok(complaint);
        }

        // GET LOGGED-IN USER COMPLAINTS
        // GET: /api/complaints/my
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyComplaints()
        {
            // Get logged-in user ID from JWT
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var complaints = await _context.Complaints
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .ToListAsync();

            return Ok(complaints);
        }


        // DELETE COMPLAINT
        // DELETE: /api/complaints/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound("Complaint not found");
            }

            // Ownership check
            if (complaint.UserId != userId)
            {
                return Forbid();
            }

            _context.Complaints.Remove(complaint);

            await _context.SaveChangesAsync();

            return Ok("Complaint deleted successfully");
        }

        // UPDATE STATUS
        // PUT: /api/complaints/1/status
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateStatusDto dto
        )
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound("Complaint not found");
            }

            complaint.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(complaint);
        }

        [HttpGet("paged")]
public async Task<ActionResult<PagedResultDto<ComplaintListDto>>> GetPagedComplaints(
    [FromQuery] ComplaintQueryParametersDto queryParams)
{
    var query = _context.Complaints.AsQueryable();

    // Filtering
    if (!string.IsNullOrWhiteSpace(queryParams.Status))
    {
        query = query.Where(c => c.Status == queryParams.Status);
    }

    // Search
    if (!string.IsNullOrWhiteSpace(queryParams.Search))
    {
        query = query.Where(c =>
            c.Title.Contains(queryParams.Search));
    }

    // Sorting
    query = queryParams.SortBy.ToLower() switch
    {
        "title" => queryParams.Descending
            ? query.OrderByDescending(c => c.Title)
            : query.OrderBy(c => c.Title),

        _ => queryParams.Descending
            ? query.OrderByDescending(c => c.CreatedAt)
            : query.OrderBy(c => c.CreatedAt)
    };

    var totalCount = await query.CountAsync();

    var complaints = await query
        .Skip((queryParams.Page - 1) * queryParams.PageSize)
        .Take(queryParams.PageSize)
        .Select(c => new ComplaintListDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        })
        .ToListAsync();
var response = new PagedResultDto<ComplaintListDto>
{
    Items = complaints,
    TotalCount = totalCount,
    Page = queryParams.Page,
    PageSize = queryParams.PageSize,
    TotalPages = (int)Math.Ceiling(
        totalCount / (double)queryParams.PageSize)
};

return Ok(response);
}
    }

    
}