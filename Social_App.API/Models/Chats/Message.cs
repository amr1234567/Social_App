using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Social_App.API.Models.Chats
{
    public class Message
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; } = DateTime.UtcNow;
        public int ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        [JsonIgnore]
        public Conversation Conversation { get; set; }
    }
}
