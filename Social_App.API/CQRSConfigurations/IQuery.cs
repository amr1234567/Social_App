using MediatR;

namespace Social_App.API.CQRSConfigurations
{
    public interface IQuery : IQuery<Unit>
    {

    }

    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {
    }
}
