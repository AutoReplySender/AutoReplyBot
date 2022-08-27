#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models;

public class Result<T>
{
    [JsonPropertyName("result_code")] public int ResultCode { get; set; }

    [JsonPropertyName("result_data")] public T ResultData { get; set; }
}