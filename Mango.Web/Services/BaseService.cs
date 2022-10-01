using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mango.Web.Services;

public class BaseService
{
    private readonly IHttpClientFactory _httpClient;

    public ResponseDto responseModel { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> SendAsync<T>(ApiRequest request)
    {
        try
        {
            var client = _httpClient.CreateClient("MangoAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            client.DefaultRequestHeaders.Clear();

            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(request.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
            }

            HttpResponseMessage response = null;

            message.Method = request.ApiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };

            response = await client.SendAsync(message);

            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);

            return apiResponseDto;

        }
        catch (Exception ex)
        {
            var responseDto = new ResponseDto
            {
                Message = "Error",
                Errors = new List<string> { ex.Message },
                IsSuccess = false
            };

            var response = JsonConvert.SerializeObject(responseDto);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(response);
            return apiResponseDto;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}
