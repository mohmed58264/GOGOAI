
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class WorkProofUploadRequest
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string Description { get; set; }
    }
}
