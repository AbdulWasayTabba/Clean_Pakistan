using System;

namespace Clean_Pakistan.API.Models
{
    public class CivicIssue
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., Pothole, Garbage, Water Shortage

        // Location Intelligence
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string ImageUrl { get; set; } = string.Empty; // Stores path to user-submitted photos
        public string Status { get; set; } = "Pending"; // Pending, Verified, Resolved
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key link back to the Citizen who reported it
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}