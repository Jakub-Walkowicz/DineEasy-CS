using DineEasy.SharedKernel.Models.Table;
using DineEasy.Application.DTOs.Table;
using DineEasy.BlazorServer.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace DineEasy.BlazorServer.Services;

public class TableApiClient : ITableApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TableApiClient> _logger;
    private readonly AuthStateService _authStateService;
    private readonly JsonSerializerOptions _jsonOptions;

    public TableApiClient(HttpClient httpClient, ILogger<TableApiClient> logger, AuthStateService authStateService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _authStateService = authStateService;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private void SetAuthorizationHeader()
    {
        if (!string.IsNullOrEmpty(_authStateService.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authStateService.Token);
        }
    }

    public async Task<IEnumerable<TableDto>> GetAllTablesAsync()
    {
        try
        {
            SetAuthorizationHeader();
            
            _logger.LogInformation("Fetching all tables from API");
            
            var response = await _httpClient.GetAsync("/api/tables");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tables = JsonSerializer.Deserialize<List<TableDto>>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully fetched {Count} tables", tables?.Count ?? 0);
                return tables ?? new List<TableDto>();
            }
            
            _logger.LogWarning("Failed to fetch tables. Status: {StatusCode}", response.StatusCode);
            return new List<TableDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching tables");
            throw;
        }
    }

    public async Task<TableDto?> GetTableByIdAsync(int id)
    {
        try
        {
            SetAuthorizationHeader();
            
            _logger.LogInformation("Fetching table with ID: {TableId}", id);
            
            var response = await _httpClient.GetAsync($"/api/tables/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var table = JsonSerializer.Deserialize<TableDto>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully fetched table with ID: {TableId}", id);
                return table;
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Table with ID {TableId} not found", id);
                return null;
            }
            
            _logger.LogWarning("Failed to fetch table with ID {TableId}. Status: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching table with ID: {TableId}", id);
            throw;
        }
    }

    public async Task<TableDto?> CreateTableAsync(CreateTableDto dto)
    {
        try
        {
            SetAuthorizationHeader();
            
            _logger.LogInformation("Creating new table with number: {TableNumber}", dto.TableNumber);
            
            var response = await _httpClient.PostAsJsonAsync("/api/tables", dto);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createdTable = JsonSerializer.Deserialize<TableDto>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully created table with ID: {TableId}", createdTable?.Id);
                return createdTable;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to create table. Status: {StatusCode}, Error: {Error}", 
                response.StatusCode, errorContent);
            
            throw new HttpRequestException($"Failed to create table: {response.StatusCode} - {errorContent}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating table");
            throw;
        }
    }

    public async Task<bool> UpdateTableAsync(int id, UpdateTableDto dto)
    {
        try
        {
            SetAuthorizationHeader();
            
            _logger.LogInformation("Updating table with ID: {TableId}", id);
            
            var response = await _httpClient.PutAsJsonAsync($"/api/tables/{id}", dto);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully updated table with ID: {TableId}", id);
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to update table with ID {TableId}. Status: {StatusCode}, Error: {Error}", 
                id, response.StatusCode, errorContent);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            
            throw new HttpRequestException($"Failed to update table: {response.StatusCode} - {errorContent}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating table with ID: {TableId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteTableAsync(int id)
    {
        try
        {
            SetAuthorizationHeader();
            
            _logger.LogInformation("Deleting table with ID: {TableId}", id);
            
            var response = await _httpClient.DeleteAsync($"/api/tables/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully deleted table with ID: {TableId}", id);
                return true;
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Table with ID {TableId} not found for deletion", id);
                return false;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to delete table with ID {TableId}. Status: {StatusCode}, Error: {Error}", 
                id, response.StatusCode, errorContent);
            
            throw new HttpRequestException($"Failed to delete table: {response.StatusCode} - {errorContent}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting table with ID: {TableId}", id);
            throw;
        }
    }
}