using MediatR;

namespace Social_App.API.CQRSConfigurations
{
    public interface IQueryHandler<in TRequest>
        : IQueryHandler<TRequest, Unit>
        where TRequest : IQuery<Unit>
    {

    }

    public interface IQueryHandler<in TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
