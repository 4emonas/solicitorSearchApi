using System.Text.Json.Serialization;

namespace SolicitorSearch.Common.Models
{
    public class Solicitor
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string? ExtraInfoPage { get; set; }
        public decimal? Rating { get; set; }
        public int? NumberOfReviews { get; set; }
        public bool IsNew { get; set; }
        [JsonIgnore]
        public bool IsSmall { get; set; }
    }
}
