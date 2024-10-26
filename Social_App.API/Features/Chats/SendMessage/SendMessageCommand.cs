using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Chats.SendMessage
{
    public class SendMessageCommand : ICommand<SendMessageCommandResponse>
    {
        public int ConversationId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
