using Newtonsoft.Json;

namespace Domain.Models;

public record ErrorResponse(
    [property: JsonProperty("message")]
    string Message);