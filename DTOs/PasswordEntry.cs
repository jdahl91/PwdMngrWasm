namespace PwdMngrWasm.DTOs
{
    public class PasswordEntry
    {
        public int EntryId { get; set; }
        public int UserId { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
