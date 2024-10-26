using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;

namespace Social_App.API.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationContext _context;

        public ConversationService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task AddConversation(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task<Conversation> FindConversationById(int Id)
        {
            var conversation = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == Id);
            return conversation;
        }

        public async Task<Conversation> FindConversationByUsersId(Guid userId1, Guid userId2)
        {
            var conversation = await _context.Conversations.FirstOrDefaultAsync(c => (c.userId1 == userId1 && c.userId2 == userId2) || 
            (c.userId1 == userId2 && c.userId2 == userId1));
            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetUserConversations(Guid userId)
        {
            IEnumerable<Conversation> conversations = await _context.Conversations.Where(c => c.userId1 == userId || c.userId2 == userId).ToListAsync();
            return conversations;
        }

        public async Task<bool> UpdateConversation(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
