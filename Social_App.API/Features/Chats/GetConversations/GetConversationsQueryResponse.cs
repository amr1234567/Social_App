namespace Social_App.API.Features.Chats.GetConversations
{
    public class GetConversationsQueryResponse
    {
        public int ConversationId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public string LastMessage { get; set; }

    }
}
