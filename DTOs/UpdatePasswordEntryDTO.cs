namespace PwdMngrWasm.DTOs
{
    public class UpdatePasswordEntryDTO
    {
        public UpdatePasswordEntryDTO() { }
        public UpdatePasswordEntryDTO(PasswordEntry entry)
        {
            this.EntryId = entry.EntryId;
            this.Url = entry.Url ?? string.Empty;
            this.Name = entry.Name ?? string.Empty;
            this.Note = entry.Note ?? string.Empty;
            this.Username = entry.Username ?? string.Empty;
            this.Password = entry.Password ?? string.Empty;
        }
        public Guid EntryId { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
