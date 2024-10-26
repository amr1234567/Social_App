using Social_App.API.CQRSConfigurations;
using Social_App.API.Models.Helpers;

namespace Social_App.API.Features.Chats.AddConversation
{
    public class AddConversationCommand : ICommand<AddConversationCommandResponse>
    {
        public Guid userId { get; set; }
    }
}
