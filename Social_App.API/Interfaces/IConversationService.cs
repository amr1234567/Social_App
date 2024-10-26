using Social_App.API.Models.Chats;

namespace Social_App.API.Interfaces
{
    public interface IConversationService
    {
        Task AddConversation(Conversation conversation);
        Task<Conversation> FindConversationByUsersId(Guid userId1 , Guid userId2);
        Task<Conversation> FindConversationById(int Id);

        Task<IEnumerable<Conversation>> GetUserConversations(Guid userId);
        Task<bool> UpdateConversation(Conversation conversation);
    }
}
