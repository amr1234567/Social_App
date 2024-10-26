using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;
using Social_App.API.Models.Helpers;
using Social_App.API.Models.Identity;
using System.Security.Claims;

namespace Social_App.API.Features.Chats.GetConversations
{
    public class GetConversationsQueryHandler : IQueryHandler<GetConversationsQuery, List<GetConversationsQueryResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly IConversationService _conversationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetConversationsQueryHandler(ApplicationContext context , IConversationService conversationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _conversationService = conversationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetConversationsQueryResponse>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            IEnumerable<Conversation> conversations = await _conversationService.GetUserConversations(currentUserId);

            List<GetConversationsQueryResponse> result = new List<GetConversationsQueryResponse>();

            foreach(var conversation in conversations)
            {
                Guid anotherUserId = Guid.Empty;

                if(conversation.userId1 != currentUserId)
                    anotherUserId = conversation.userId1;
                else
                    anotherUserId = conversation.userId2;

                User user = await _context.Users.FindAsync(anotherUserId);

                result.Add(new GetConversationsQueryResponse
                {
                    ConversationId = conversation.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LastMessage = conversation.LastMessage,
                    LastUpdated = conversation.LastUpdated,
                    UserId = user.Id,
                });
            }

            return result;
        }
    }
}
