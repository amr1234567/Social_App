using System.ComponentModel.DataAnnotations;

namespace Social_App.API.Models.Chats
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public Guid userId1 { get; set; } 
        public Guid userId2 { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string LastMessage { get; set; } = "Start Conversation";
        public List<Message> Messages { get; set; }
    }
}
