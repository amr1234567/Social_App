using Microsoft.AspNetCore.SignalR;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;
using Social_App.API.SignalR;
using System.Security.Claims;

namespace Social_App.API.Features.Chats.SendMessage
{
    public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, SendMessageCommandResponse>
    {
        private readonly IMessageService _messageService;
        private readonly IConversationService _conversationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendMessageCommandHandler(IMessageService messageService , IConversationService conversationService , IHttpContextAccessor httpContextAccessor)
        {
            _messageService = messageService;
            _conversationService = conversationService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SendMessageCommandResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var conversation = await _conversationService.FindConversationById(request.ConversationId);

            Message message = new Message
            {
                Content = request.Content,
                ConversationId = request.ConversationId,
                ReceiverId = request.ReceiverId,
                SendAt = DateTime.UtcNow,
                SenderId = currentUserId,
            };

            await _messageService.AddMessage(message);

            conversation.LastUpdated = DateTime.UtcNow;
            conversation.LastMessage = message.Content;

            await _conversationService.UpdateConversation(conversation);

            return new SendMessageCommandResponse
            {
                Success = true,
                Message = "Message Added Successfully",
                MessageId = message.Id,
                Content = message.Content,
                ConversationId = message.ConversationId,
                ReceiverId = message.ReceiverId,
                SendAt = message.SendAt,
                SenderId = message.SenderId,
            };
        }
    }
}
