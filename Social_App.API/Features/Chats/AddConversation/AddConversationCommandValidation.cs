using FluentValidation;

namespace Social_App.API.Features.Chats.AddConversation
{
    public class AddConversationCommandValidation : AbstractValidator<AddConversationCommand>
    {
        public AddConversationCommandValidation()
        {
            RuleFor(c => c.userId).NotNull().NotEmpty().WithMessage("User Id Can't be Null or Empty!!!");
        }
    }
}
