
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FixoraBackend.Services
{
    public class FCMService
    {
        private readonly HttpClient _httpClient;
        private readonly string _fcmServerKey;

        public FCMService(HttpClient httpClient, string fcmServerKey)
        {
            _httpClient = httpClient;
            _fcmServerKey = fcmServerKey;
        }

        public async Task<bool> SendPushNotificationAsync(string deviceToken, string title, string body)
        {
            var payload = new
            {
                to = deviceToken,
                notification = new
                {
                    title = title,
                    body = body
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            request.Headers.TryAddWithoutValidation("Authorization", $"key={_fcmServerKey}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FCM Error: " + ex.Message);
                return false;
            }
        }
    }
}