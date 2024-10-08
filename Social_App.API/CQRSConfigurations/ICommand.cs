using MediatR;

namespace Social_App.API.CQRSConfigurations
{
    public interface ICommand : ICommand<Unit>
    {

    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
