namespace Mango.Web.Models.Dtos;

public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;
    public object Response { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; }
}
