using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;

namespace Social_App.API.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationContext _context;

        public MessageService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            return true;
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationId(int conversationId)
        {
            var messages = await _context.Messages.Where(m => m.ConversationId == conversationId).ToListAsync();
            return messages;
        }
    }
}
