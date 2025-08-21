namespace FixoraBackend.DTOs
{
    using System.ComponentModel.DataAnnotations;

    namespace FixoraBackend.DTOs
    {
        public class UpdateLocationRequest
        {
            [Required]
            public double Latitude { get; set; }

            [Required]
            public double Longitude { get; set; }
        }
    }
}
