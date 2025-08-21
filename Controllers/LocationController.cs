using FixoraBackend.DTOs.FixoraBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using FixoraBackend.Services;

namespace FixoraBackend.Controllers
{
    namespace FixoraBackend.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class LocationController : ControllerBase
        {
            private readonly AppDbContext _context;

            public LocationController(AppDbContext context)
            {
                _context = context;
            }

            [Authorize(Roles = "Client,Provider")]
            [HttpPost("update")]
            public async Task<IActionResult> UpdateLocation(UpdateLocationRequest request)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                await _locationHistoryService.SaveProviderLocationAsync(user.Id, request.Latitude, request.Longitude, request.OrderId);

                if (user == null) return NotFound(new { message = "User not found" });

                user.Latitude = request.Latitude;
                user.Longitude = request.Longitude;
                user.LastLocationUpdate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Location updated successfully" });
            }
        }
    }
}




