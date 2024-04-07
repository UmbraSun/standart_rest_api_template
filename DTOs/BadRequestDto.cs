using System.Text.Json.Serialization;

namespace DTOs
{
    public sealed class BadRequestDto
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }

        [JsonPropertyName("devCode")]
        public int? DevCode { get; set; }
    }
}
