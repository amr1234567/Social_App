using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Asn1.Ocsp;
using Social_App.API.Features.Chats.SendMessage;
using Social_App.API.Interfaces;
using Social_App.API.Models.Chats;
using Social_App.API.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Social_App.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IMediator mediator, ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessage(int conversationId , string receiverId, string content)
        {
            var command = new SendMessageCommand
            {
                ConversationId = conversationId,
                ReceiverId = Guid.Parse(receiverId),
                Content = content
            };
            var response = await _mediator.Send(command);

            var connectionIds = await _context.UsersConnections 
                .Where(uc => uc.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) || uc.UserId == receiverId)
                .Select(uc => uc.ConnectionId)
                .ToListAsync();

            var message = new Message
            {
                Id = response.MessageId,
                SendAt = response.SendAt,
                SenderId = response.SenderId,
                ReceiverId = response.ReceiverId,
                Content = content,
                ConversationId = conversationId,
            };

            var responseStr = JsonSerializer.Serialize(message);
            await Clients.Clients(connectionIds).SendAsync("RecieveMessage", responseStr);
        }

        public override async Task OnConnectedAsync()
        {
            await _context.UsersConnections.AddAsync(new UserConnection { UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) , ConnectionId = Context.ConnectionId});
            await _context.SaveChangesAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userConnection = _context.UsersConnections.Where(uc => uc.ConnectionId == Context.ConnectionId).FirstOrDefault();
            _context.UsersConnections.Remove(userConnection);
            await _context.SaveChangesAsync();
        }
    }
}
