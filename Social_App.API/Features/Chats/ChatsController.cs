using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_App.API.Features.Chats.AddConversation;
using Social_App.API.Features.Chats.GetConversations;
using Social_App.API.Features.Chats.LoadConversation;
using Social_App.API.Features.Chats.SendMessage;

namespace Social_App.API.Features.Chats
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("AddConversation")]
        public async Task<IActionResult> AddConversation(AddConversationCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetConversations")]
        public async Task<IActionResult> GetConversations()
        {
            var request = new GetConversationsQuery();
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(SendMessageCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("LoadConversation/{userId}/{conversationId}")]
        public async Task<IActionResult> LoadConversationWithConversationId(string userId, [FromRoute]int conversationId)
        {
            var request = new LoadConversationQuery
            {
                userId = Guid.Parse(userId),
                ConversationId = conversationId
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("LoadConversation/{userId}")]
        public async Task<IActionResult> LoadConversationWithUserId(string userId)
        {
            var request = new LoadConversationQuery
            {
                userId = Guid.Parse(userId),
                ConversationId = 0
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }


    }
}
