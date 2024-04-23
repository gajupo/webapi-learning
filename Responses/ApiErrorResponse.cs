using System.Text.Json.Serialization;

namespace webapi_learning.Responses
{
    public class ApiErrorResponse<T>
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
