namespace Mango.Services.ProductAPI.Models.Dtos;

public class ResponseDto<T> where T : class
{
    public bool IsSuccess { get; set; } = true;
    public T Response { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; }
}
