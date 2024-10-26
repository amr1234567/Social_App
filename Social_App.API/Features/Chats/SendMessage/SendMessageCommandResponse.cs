namespace Social_App.API.Features.Chats.SendMessage
{
    public class SendMessageCommandResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int MessageId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; } = DateTime.UtcNow;
        public int ConversationId { get; set; }
    }
}
