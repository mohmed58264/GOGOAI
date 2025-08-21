using FixoraBackend.Data.FixoraBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FixoraBackend.Data
{

    public class ApplicationUser : IdentityUser
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime? LastLocationUpdate { get; set; }
    }

}