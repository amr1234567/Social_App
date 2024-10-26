using Social_App.API.Models.Chats;

namespace Social_App.API.Features.Chats.LoadConversation
{
    public class LoadConversationQueryResponse
    {
        public int ConversationId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
