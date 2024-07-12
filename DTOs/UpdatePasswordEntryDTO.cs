using System.Text.Json.Serialization;

namespace PwdMngrWasm.DTOs
{
    public class UpdatePasswordEntryDTO
    {
        public UpdatePasswordEntryDTO(PasswordEntry entry)
        {
            this.EntryId = entry.EntryId.ToString();
            this.Url = entry.Url ?? string.Empty;
            this.Name = entry.Name ?? string.Empty;
            this.Note = entry.Note ?? string.Empty;
            this.Username = entry.Username ?? string.Empty;
            this.Password = entry.Password ?? string.Empty;
        }
        [JsonPropertyName("entryId")]
        public string EntryId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("note")]
        public string Note { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
