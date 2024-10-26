using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Chats.LoadConversation
{
    public class LoadConversationQuery : IQuery<LoadConversationQueryResponse>
    {
        public Guid userId { get; set; }
        public int ConversationId { get; set; }
    }
}
