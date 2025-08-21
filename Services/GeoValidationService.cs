using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using FixoraBackend.Controllers.FixoraBackend.Controllers;
using FixoraBackend.DTOs.FixoraBackend.DTOs;
using FixoraBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FixoraBackend.Services
{
    public class GeoValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GeoValidationService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> ValidateCoordinatesAsync(double latitude, double longitude)
        {
            // التحقق من النطاق العام للإحداثيات (منع قيم مزيفة)
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
                return false;

            // استدعاء API للتحقق من الإحداثيات (Google Maps أو OpenStreetMap)
            var apiKey = _config["GoogleMaps:ApiKey"];
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // إذا لم يتم العثور على عنوان صالح
            if (!doc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                return false;

            return true;
        }
    }
}
