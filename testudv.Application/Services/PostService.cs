using System.Text.Json;
using Microsoft.Extensions.Configuration;
using testudv.Application.Dtos;
using testudv.Domain.Entities;

namespace testudv.Application.Services;

public class PostService
{
    private readonly HttpClient _httpClient;
    private readonly string _serviceToken;
    private readonly string _apiVersion = "5.199";

    public PostService(HttpClient httpClient, string serviceToken)
    {
        _httpClient = httpClient;
        _serviceToken = serviceToken;
    }

    public async Task<string> GetPostsAsync(string domain, int count)
    {
        var url = $"https://api.vk.com/method/wall.get?" +
                  $"domain={domain}&" +
                  $"count={count}&" +
                  $"access_token={_serviceToken}&" +
                  $"v={_apiVersion}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("Пустой ответ");
            }

            using var jsonDocument = JsonDocument.Parse(content);
            var root = jsonDocument.RootElement;

            if (!root.TryGetProperty("response", out var responseElement) || 
                !responseElement.TryGetProperty("items", out var itemsElement) ||
                itemsElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Неверная структура JSON");
            }

            var texts = new List<string>();
            foreach (var item in itemsElement.EnumerateArray())
            {
                if (item.TryGetProperty("text", out var textElement) &&
                    textElement.ValueKind == JsonValueKind.String)
                {
                    var text = textElement.GetString().Trim().Replace("\n", "");
                    if (!string.IsNullOrEmpty(text))
                    {
                        texts.Add(text);
                    }
                }
            }
            return string.Join(" ",texts).ToLower();
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException("Ошибка отправки запроса", ex);
        }
        catch (JsonException ex)
        {
            throw new ApplicationException("Ошибка парсинга JSON", ex);
        }
    }
}