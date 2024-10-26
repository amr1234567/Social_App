using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;
using Social_App.API.Models.Identity;
using System.Security.Claims;

namespace Social_App.API.Features.Chats.LoadConversation
{
    public class LoadConversationQueryHandler : IQueryHandler<LoadConversationQuery, LoadConversationQueryResponse>
    {
        private readonly IMessageService _messageService;
        private readonly IConversationService _conversationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationContext _context;

        public LoadConversationQueryHandler(IMessageService messageService, IConversationService conversationService, IHttpContextAccessor httpContextAccessor, ApplicationContext context)
        {
            _messageService = messageService;
            _conversationService = conversationService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public async Task<LoadConversationQueryResponse> Handle(LoadConversationQuery request, CancellationToken cancellationToken)
        {
            Conversation conversation = new Conversation();

            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(currentUserId);

            if(request.ConversationId != 0)
            {
                conversation = await _conversationService.FindConversationById(request.ConversationId);
            }
            else
            {
                conversation = await _conversationService.FindConversationByUsersId(currentUserId, request.userId);
                if(conversation == null)
                {
                    var newConversation = new Conversation
                    {
                        userId1 = currentUserId,
                        userId2 = request.userId,
                    };

                    await _conversationService.AddConversation(newConversation);

                    conversation = newConversation;
                }
            }

            var messages = await _messageService.GetMessagesByConversationId(conversation.Id);

            return new LoadConversationQueryResponse
            {
                ConversationId = conversation.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Messages = messages.ToList(),
                UserId = currentUserId,
            };
        }
    }
}
