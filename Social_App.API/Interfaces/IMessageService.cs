using Social_App.API.Models.Chats;

namespace Social_App.API.Interfaces
{
    public interface IMessageService
    {
        Task<bool> AddMessage(Message message);
        Task<IEnumerable<Message>> GetMessagesByConversationId(int conversationId);
    }
}
