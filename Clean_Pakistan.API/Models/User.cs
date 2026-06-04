namespace Clean_Pakistan.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty; // Essential for localized apps
        public string Role { get; set; } = "Citizen"; // "Citizen" or "Admin"
        public int TrustScore { get; set; } = 10; // Used to downrank spam reporters
    }
}