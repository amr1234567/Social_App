using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;
using Social_App.API.Models.Helpers;
using Social_App.API.Models.Identity;
using System.Security.Claims;

namespace Social_App.API.Features.Chats.AddConversation
{
    public class AddConversationCommandHandler : ICommandHandler<AddConversationCommand, AddConversationCommandResponse>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationService _conversationService;

        public AddConversationCommandHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor , IConversationService conversationService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _conversationService = conversationService;
        }
        public async Task<AddConversationCommandResponse> Handle(AddConversationCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var users = await _context.Users.Where(u => u.Id == request.userId || u.Id == currentUserId).ToListAsync();
            if (users.Count < 2)
                return new AddConversationCommandResponse { Success = false, Message = "User Not Found"};

            var conversation = new Conversation
            {
                userId1 = currentUserId,
                userId2 = request.userId,
            };

            await _conversationService.AddConversation(conversation);

            return new AddConversationCommandResponse { Success = true, Message = "Conversation Added Successfully"};
        }
    }
}
