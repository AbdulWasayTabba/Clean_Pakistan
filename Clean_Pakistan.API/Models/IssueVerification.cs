using System;

namespace Clean_Pakistan.API.Models
{
    public class IssueVerification
    {
        public int Id { get; set; }
        public bool IsLegitimate { get; set; } // True = Upvote/Verify, False = Downvote/Spam
        public DateTime VerifiedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key link to the specific issue being verified
        public int IssueId { get; set; }
        public CivicIssue Issue { get; set; } = null!;

        // Foreign Key link to the Citizen performing the verification
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}