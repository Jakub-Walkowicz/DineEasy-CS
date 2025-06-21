// Services/ReservationApiClient.cs
using DineEasy.SharedKernel.Models.Reservation;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DineEasy.BlazorServer.Services
{
    public class ReservationApiClient : IReservationApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthStateService _authStateService;

        public ReservationApiClient(HttpClient httpClient, AuthStateService authStateService)
        {
            _httpClient = httpClient;
            _authStateService = authStateService;
        }

        private void SetAuthHeaders()
        {
            if (!string.IsNullOrEmpty(_authStateService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _authStateService.Token);
            }
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            SetAuthHeaders();
            var response = await _httpClient.GetAsync("api/reservations");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<ReservationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ReservationDto>();
            }
            
            return new List<ReservationDto>();
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id)
        {
            SetAuthHeaders();
            var response = await _httpClient.GetAsync($"api/reservations/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ReservationDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            
            return null;
        }

        public async Task<ReservationDto?> CreateReservationAsync(CreateReservationDto dto)
        {
            SetAuthHeaders();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/reservations", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ReservationDto>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            
            return null;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            SetAuthHeaders();
            var response = await _httpClient.DeleteAsync($"api/reservations/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}