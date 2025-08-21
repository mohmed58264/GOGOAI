using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using FixoraBackend.Data;
using FixoraBackend.DTOs;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkProofController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WorkProofController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(Roles = "Provider")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadWorkProof([FromForm] WorkProofUploadRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId && o.ProviderId == userId);
            if (order == null)
                return NotFound(new { message = "Order not found or not assigned to you" });

            // حفظ الملف
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "workproofs");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/workproofs/{fileName}";

            var proof = new WorkProof
            {
                OrderId = request.OrderId,
                ProviderId = userId,
                FileUrl = fileUrl,
                FileType = request.File.ContentType,
                Description = request.Description
            };

            _context.WorkProofs.Add(proof);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Work proof uploaded successfully", proof });
        }

        [Authorize(Roles = "Admin,Client")]
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetWorkProofs(int orderId)
        {
            var proofs = await _context.WorkProofs
                .Where(wp => wp.OrderId == orderId)
                .OrderByDescending(wp => wp.UploadedAt)
                .ToListAsync();

            return Ok(proofs);
        }
    }
}

