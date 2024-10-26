using FluentValidation;

namespace Social_App.API.Features.Chats.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(c => c.ConversationId).NotNull().NotEmpty().WithMessage("Conversation Id can't be null or empty");
            RuleFor(c => c.ReceiverId).NotNull().NotEmpty().WithMessage("Receiver Id can't be null or empty");
            RuleFor(c => c.Content).NotNull().NotEmpty().WithMessage("content of message can't be null or empty");
        }
    }
}
